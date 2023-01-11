using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Models
{
    public class Inventory
    {
        public int AddProduct(int pid,int sid,int quantity,int buying_rate,int selling_rate)
        {
            using (var context = new InventoryManagementSystemEntities())
            {
                var pids = (from Stock in context.Stock
                            select Stock.productid).ToList();
                var sids = (from Stock in context.Stock
                            select Stock.supplierid).ToList();

                bool pidFound = false;
                bool sidFound = false;
                for (int i = 0; i < pids.Count; i++)
                {
                    if(pids[i]==pid)
                    {
                        pidFound=true;
                        break;
                    }
                }
                for(int i =0;i<sids.Count;i++)
                {
                    if(sids[i]==sid)
                    {
                        sidFound = true;
                        break;
                    }
                }

                if (sidFound == true && pidFound ==true)
                {
                    var _isProduct = context.Stock.FirstOrDefault(x => x.productid == pid && x.supplierid == sid);
                    if(_isProduct!=null)
                    {
                        _isProduct.product_quantity = _isProduct.product_quantity + quantity;
                        _isProduct.selling_rate = selling_rate;
                        _isProduct.buying_rate = buying_rate;
                        _isProduct.productid = pid;
                        _isProduct.supplierid = sid;
                    }
                    return context.SaveChanges();
                }
                else
                {
                    var stock = new Stock()
                    {
                        productid = pid,
                        supplierid = sid,
                        buying_rate = buying_rate,
                        selling_rate = selling_rate,
                        product_quantity = quantity
                    };
                    context.Stock.Add(stock);
                    return context.SaveChanges();
                }
            }
        }

        public int SellProduct(int pid, int sid, int quantity)
        {
            using (var context = new InventoryManagementSystemEntities())
            {
                    var _isProduct = context.Stock.FirstOrDefault(x => x.productid == pid && x.supplierid == sid);
                    if (_isProduct != null)
                    {
                        _isProduct.product_quantity = _isProduct.product_quantity - quantity;
                        _isProduct.productid = pid;
                        _isProduct.supplierid = sid;
                    }
                    return context.SaveChanges();
            }
        }

        public int CheckInventory(int pid,int sid)
        {
            using (var context = new InventoryManagementSystemEntities())
            {
                int quantity =  (from Stock in context.Stock
                                where Stock.productid == pid && Stock.supplierid == sid
                                select Stock.product_quantity).FirstOrDefault();
                return quantity;
            }
        }
        public int CheckProduct(int pid,int sid)
        {
            using (var context = new InventoryManagementSystemEntities())
            {
                var _isProduct = context.Stock.FirstOrDefault(x => x.productid == pid && x.supplierid == sid);
                if(_isProduct!=null)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int AddUser(User model)
        {
            using (var context = new InventoryManagementSystemEntities())
            {
                var user = new User()
                {
                    username = model.username,
                    password = model.password
                };
                context.User.Add(user);
                return context.SaveChanges();
            }
        }

        public string[] ProductAndSupplier(int pid,int sid)
        {
            string[] productAndSuppliers = new string[2];
            using (var context = new InventoryManagementSystemEntities())
            {
                string pname = (from product in context.Product
                                where product.id == pid
                                select product.name).FirstOrDefault();
                productAndSuppliers[0] = pname;
            }
            using (var context = new InventoryManagementSystemEntities())
            {
                string sname = (from supplier in context.Supplier
                                where supplier.id == sid
                                select supplier.name).FirstOrDefault();
                productAndSuppliers[1] = sname;
            }
            return productAndSuppliers;
        }
    }
}