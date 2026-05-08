using GLSCM.Common.Util;
using Hichain.Common.Utilities;
using Hichain.Entity.Entities;
using Hichain.Entity.Models;
using Hichain.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hichain.Business;

/// <summary>
/// 入库单业务逻辑
/// </summary>
public class InboundBLL
{
    private InboundService _service = null;

    public InboundBLL()
    {
        _service = new InboundService();
    }

    /// <summary>
    /// 创建入库单（包含明细）
    /// </summary>
    /// <param name="inbound">入库单实体</param>
    /// <returns></returns>
    public async Task<Inbound> CreateInboundAsync(InboundParam inboundparam)
    {
        try
        {
            Inbound inbound = new Inbound();
            inbound.InboundID= new Guid("3FA85F65-5717-4562-B3FC-3C963F66AFA9");
            inbound.InboundOrderNO = inboundparam.orderno;
            inbound.AutoCodeRuleCode = "";
            inbound.InboundType = 0;
            inbound.ReceiveType = 0;
            inbound.Priority = 0;
            inbound.PutawayState = 0;
            inbound.CustomerID =new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");
            inbound.WarehouseID= new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");
            inbound.UserDefineAttributeID = new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");
            inbound.ExpInboundDate = inboundparam.expinbounddate;
            inbound.OrderNO = inboundparam.orderno;
            inbound.CreateDate = DateTime.Now;
            inbound.CreateBy = "edi";
            List<InboundPart> inboundParts = new List<InboundPart>();
            // 为明细生成主键和设置创建时间
            if (inboundparam.inboundparts != null && inboundparam.inboundparts.Any())
            {
                int i = 1;
                foreach (var part in inboundparam.inboundparts)
                {
                    InboundPart inboundpart = new InboundPart();
                    inboundpart.InboundPartID = Guid.NewGuid();
                    inboundpart.InboundID = inbound.InboundID;
                    inboundpart.InboundOrderNO = inbound.InboundOrderNO;
                    inboundpart.CreateDate = DateTime.Now;
                    inboundpart.CreateBy = inbound.CreateBy;
                    inboundpart.PartNO= part.PartNO;
                    inboundpart.SeqNO = i;
                    inboundpart.CustomerID= inbound.CustomerID;
                    inboundpart.PartDesc= part.PartDesc;
                    inboundpart.ExpEAQty= part.ExpEAQty;
                    inboundpart.PackRuleID=new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");
                    inboundpart.PutawayRuleID= new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");
                    inboundpart.LotAttributeRuleID= new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");
                    inboundpart.OrderNO= inbound.OrderNO;
                    inboundpart.CreateDate= DateTime.Now;
                    inboundpart.CreateBy = inbound.CreateBy;
                    inboundParts.Add(inboundpart);
                }
            }
            inbound.InboundParts = inboundParts;
            await _service.CreateInboundAsync(inbound);
            return inbound;
        }
        catch (Exception ex)
        {
            WeixinConfig wcfg = new WeixinConfig();
            WeixinPushHelp.SendToMessage(wcfg, "创建入库单报错：" + ex.Message);
            LogHelper.Error("创建入库单失败", ex);
            throw;
        }
    }
}
