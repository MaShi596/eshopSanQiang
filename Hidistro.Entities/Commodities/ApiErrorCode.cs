using System;
namespace Hidistro.Entities.Commodities
{
	public enum ApiErrorCode
	{
		Success = 100,
		Paramter_Error,
		Format_Eroor,
		Signature_Error,
		Empty_Error,
		NoExists_Error,
		Exists_Error,
		Paramter_Diffrent,
		Group_Error,
		NoPay_Error,
		NoShippingMode,
		ShipingOrderNumber_Error,
		Session_Empty = 200,
		Session_Error,
		Session_TimeOut,
		Username_Exist,
		Ban_Register,
		SaleState_Error = 300,
		Stock_Error,
		Unknown_Error = 999
	}
}
