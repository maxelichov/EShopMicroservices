using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public async override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
        .Coupons
        .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon == null)
        {
            logger.LogInformation($"Found no coupon with ProductName: {request.ProductName}. returning an empty coupon.");

            return new CouponModel { ProductName = "No discount", Amount = 0, Description = "No discount" };
        }

        logger.LogInformation($"Discount is retrieved from ProductName: {coupon.ProductName} with Amount: {coupon.Amount} and Description: {coupon.Description}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "invalid request.."));
        }

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount is successfully created. ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "invalid request.."));
        }

        logger.LogInformation($"Starting to update discount ProductName: {request.Coupon.ProductName}, Amount: {request.Coupon.Amount}, Description: {request.Coupon.Description}");

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Successfully updated discount. After update - ProductName: {coupon.ProductName}, Amount: {coupon.Amount}, Description: {coupon.Description}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation($"Starting to delete discount. ProductName: {request.Coupon.ProductName}");

        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.Coupon.ProductName);

        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "not found"));
        }

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Discount successfully deleted. ProductName: {coupon.ProductName}");

        return new DeleteDiscountResponse { IsSuccess = true};
    }
}
