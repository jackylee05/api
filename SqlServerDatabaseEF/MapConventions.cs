///*********************************************************************
///Copyright (C) 2016-2019 56-cloud.com 
///Project Name          : 56SCM 
///Create By             : hkwong.wang@qq.com
///Create Date           : 2018-11-11
///Last Updated By       :
///Last Updated Date     :
///Version               :2018-11-11
///*********************************************************************

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;

namespace Hichain.DataAccess.Data.EF
{
    /// <summary>
    /// 主键约定，把属性Id当做数据库主键.
    /// </summary>
    public class PrimaryKeyConvention
    {
        /// <summary>
        /// The SetPrimaryKey.
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/>.</param>
        /// <param name="clrType">The clrType<see cref="Type"/>.</param>
        public static void SetPrimaryKey(ModelBuilder builder, Type clrType)
        {
            // 检查实体是否已经通过 [Key] 特性定义了主键
            var keyProperty = clrType.GetProperties()
                .FirstOrDefault(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());

            if (keyProperty != null)
            {
                // 如果已经有 [Key] 特性，使用该属性作为主键
                builder.Entity(clrType).HasKey(keyProperty.Name);
            }
            else
            {
                // 否则尝试使用 "Id" 作为主键（默认约定）
                var idProperty = clrType.GetProperty("Id");
                if (idProperty != null)
                {
                    builder.Entity(clrType).HasKey("Id");
                }
            }
        }
    }

    /// <summary>
    /// 列名约定，比如属性ParentId，映射到数据库字段parent_id.
    /// </summary>
    [Obsolete]
    public class ColumnConvention
    {
        /// <summary>
        /// The SetColumnName.
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/>.</param>
        /// <param name="entityName">The entityName<see cref="string"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        public static void SetColumnName(ModelBuilder modelBuilder, string entityName, string propertyName)
        {
            StringBuilder sbField = new StringBuilder();
            char[] charArr = propertyName.ToCharArray();
            int iCapital = 0; // 把属性第一个开始的大写字母转成小写，直到遇到了第1个小写字母，因为数据库里面是小写的
            while (iCapital < charArr.Length)
            {
                if (charArr[iCapital] >= 'A' && charArr[iCapital] <= 'Z')
                {
                    charArr[iCapital] = (char)(charArr[iCapital] + 32);
                }
                else
                {
                    break;
                }
                iCapital++;
            }
            for (int i = 0; i < charArr.Length; i++)
            {
                if (charArr[i] >= 'A' && charArr[i] <= 'Z')
                {
                    charArr[i] = (char)(charArr[i] + 32);
                    sbField.Append("_" + charArr[i]);
                }
                else
                {
                    sbField.Append(charArr[i]);
                }
            }
            modelBuilder.Entity(entityName).Property(propertyName).HasColumnName(sbField.ToString());
        }
    }
}
