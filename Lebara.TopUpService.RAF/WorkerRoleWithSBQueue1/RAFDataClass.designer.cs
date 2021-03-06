﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkerRoleWithSBQueue1
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="LebaraTopUp")]
	public partial class RAFDataClassDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public RAFDataClassDataContext() : 
				base(global::WorkerRoleWithSBQueue1.Properties.Settings.Default.LebaraTopUpConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public RAFDataClassDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RAFDataClassDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RAFDataClassDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RAFDataClassDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.spFetchRAFDetails")]
		public ISingleResult<spFetchRAFDetailsResult> spFetchRAFDetails([global::System.Data.Linq.Mapping.ParameterAttribute(Name="MSISDN", DbType="VarChar(25)")] string mSISDN)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), mSISDN);
			return ((ISingleResult<spFetchRAFDetailsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.spUpdateRAFDetails")]
		public int spUpdateRAFDetails([global::System.Data.Linq.Mapping.ParameterAttribute(Name="RefererMSISDN", DbType="NVarChar(20)")] string refererMSISDN, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="RefereeMSISDN", DbType="NVarChar(20)")] string refereeMSISDN, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferalCode", DbType="NVarChar(50)")] string referalCode, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="TransactionId", DbType="VarChar(50)")] string transactionId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="RefererTopupCount", DbType="Int")] System.Nullable<int> refererTopupCount, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="RefereeTopupCount", DbType="Int")] System.Nullable<int> refereeTopupCount)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), refererMSISDN, refereeMSISDN, referalCode, transactionId, refererTopupCount, refereeTopupCount);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.sp_InsertRAFAllTransaction")]
		public int sp_InsertRAFAllTransaction(
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferrerMSISDN", DbType="NVarChar(20)")] string referrerMSISDN, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferrerCountryCode", DbType="NVarChar(10)")] string referrerCountryCode, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferrerCurrency", DbType="NVarChar(10)")] string referrerCurrency, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferrerTopupCount", DbType="Int")] System.Nullable<int> referrerTopupCount, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferreeMSISDN", DbType="NVarChar(20)")] string referreeMSISDN, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferreeCountryCode", DbType="NVarChar(10)")] string referreeCountryCode, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferreeCurrency", DbType="NVarChar(10)")] string referreeCurrency, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferalCode", DbType="NVarChar(50)")] string referalCode, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferreeTopupCount", DbType="Int")] System.Nullable<int> referreeTopupCount, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferreeAwardedAmount", DbType="Decimal(5,2)")] System.Nullable<decimal> referreeAwardedAmount, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ReferrerAwardedAmount", DbType="Decimal(5,2)")] System.Nullable<decimal> referrerAwardedAmount, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="TopUpDateTime", DbType="DateTime")] System.Nullable<System.DateTime> topUpDateTime, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsSuccessful", DbType="Bit")] System.Nullable<bool> isSuccessful, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ErrorDescription", DbType="VarChar(1)")] string errorDescription, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="VendorName", DbType="VarChar(50)")] string vendorName, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="TransactionID", DbType="VarChar(50)")] string transactionID, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ProductAwarded", DbType="VarChar(50)")] string productAwarded)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), referrerMSISDN, referrerCountryCode, referrerCurrency, referrerTopupCount, referreeMSISDN, referreeCountryCode, referreeCurrency, referalCode, referreeTopupCount, referreeAwardedAmount, referrerAwardedAmount, topUpDateTime, isSuccessful, errorDescription, vendorName, transactionID, productAwarded);
			return ((int)(result.ReturnValue));
		}
	}
	
	public partial class spFetchRAFDetailsResult
	{
		
		private string _ReferrerMSISDN;
		
		private string _ReferrerCountryCode;
		
		private string _ReferrerCurrency;
		
		private int _ReferrerTopupCount;
		
		private string _ReferreeMSISDN;
		
		private string _ReferreeCountryCode;
		
		private string _ReferreeCurrency;
		
		private string _ReferalCode;
		
		private int _ReferreeTopupCount;
		
		private System.Nullable<decimal> _ReferreeAwardedAmount;
		
		private System.Nullable<decimal> _ReferrerAwardedAmount;
		
		private System.DateTime _TopUpDateTime;
		
		private bool _IsSuccessful;
		
		private string _ErrorDescription;
		
		private string _VendorName;
		
		private string _TransactionID;
		
		private string _ProductAwarded;
		
		public spFetchRAFDetailsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferrerMSISDN", DbType="NVarChar(20) NOT NULL", CanBeNull=false)]
		public string ReferrerMSISDN
		{
			get
			{
				return this._ReferrerMSISDN;
			}
			set
			{
				if ((this._ReferrerMSISDN != value))
				{
					this._ReferrerMSISDN = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferrerCountryCode", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string ReferrerCountryCode
		{
			get
			{
				return this._ReferrerCountryCode;
			}
			set
			{
				if ((this._ReferrerCountryCode != value))
				{
					this._ReferrerCountryCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferrerCurrency", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string ReferrerCurrency
		{
			get
			{
				return this._ReferrerCurrency;
			}
			set
			{
				if ((this._ReferrerCurrency != value))
				{
					this._ReferrerCurrency = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferrerTopupCount", DbType="Int NOT NULL")]
		public int ReferrerTopupCount
		{
			get
			{
				return this._ReferrerTopupCount;
			}
			set
			{
				if ((this._ReferrerTopupCount != value))
				{
					this._ReferrerTopupCount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferreeMSISDN", DbType="NVarChar(20) NOT NULL", CanBeNull=false)]
		public string ReferreeMSISDN
		{
			get
			{
				return this._ReferreeMSISDN;
			}
			set
			{
				if ((this._ReferreeMSISDN != value))
				{
					this._ReferreeMSISDN = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferreeCountryCode", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string ReferreeCountryCode
		{
			get
			{
				return this._ReferreeCountryCode;
			}
			set
			{
				if ((this._ReferreeCountryCode != value))
				{
					this._ReferreeCountryCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferreeCurrency", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string ReferreeCurrency
		{
			get
			{
				return this._ReferreeCurrency;
			}
			set
			{
				if ((this._ReferreeCurrency != value))
				{
					this._ReferreeCurrency = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferalCode", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string ReferalCode
		{
			get
			{
				return this._ReferalCode;
			}
			set
			{
				if ((this._ReferalCode != value))
				{
					this._ReferalCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferreeTopupCount", DbType="Int NOT NULL")]
		public int ReferreeTopupCount
		{
			get
			{
				return this._ReferreeTopupCount;
			}
			set
			{
				if ((this._ReferreeTopupCount != value))
				{
					this._ReferreeTopupCount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferreeAwardedAmount", DbType="Decimal(5,2)")]
		public System.Nullable<decimal> ReferreeAwardedAmount
		{
			get
			{
				return this._ReferreeAwardedAmount;
			}
			set
			{
				if ((this._ReferreeAwardedAmount != value))
				{
					this._ReferreeAwardedAmount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferrerAwardedAmount", DbType="Decimal(5,2)")]
		public System.Nullable<decimal> ReferrerAwardedAmount
		{
			get
			{
				return this._ReferrerAwardedAmount;
			}
			set
			{
				if ((this._ReferrerAwardedAmount != value))
				{
					this._ReferrerAwardedAmount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TopUpDateTime", DbType="DateTime NOT NULL")]
		public System.DateTime TopUpDateTime
		{
			get
			{
				return this._TopUpDateTime;
			}
			set
			{
				if ((this._TopUpDateTime != value))
				{
					this._TopUpDateTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsSuccessful", DbType="Bit NOT NULL")]
		public bool IsSuccessful
		{
			get
			{
				return this._IsSuccessful;
			}
			set
			{
				if ((this._IsSuccessful != value))
				{
					this._IsSuccessful = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorDescription", DbType="VarChar(MAX)")]
		public string ErrorDescription
		{
			get
			{
				return this._ErrorDescription;
			}
			set
			{
				if ((this._ErrorDescription != value))
				{
					this._ErrorDescription = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VendorName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string VendorName
		{
			get
			{
				return this._VendorName;
			}
			set
			{
				if ((this._VendorName != value))
				{
					this._VendorName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TransactionID", DbType="VarChar(50)")]
		public string TransactionID
		{
			get
			{
				return this._TransactionID;
			}
			set
			{
				if ((this._TransactionID != value))
				{
					this._TransactionID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductAwarded", DbType="VarChar(50)")]
		public string ProductAwarded
		{
			get
			{
				return this._ProductAwarded;
			}
			set
			{
				if ((this._ProductAwarded != value))
				{
					this._ProductAwarded = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
