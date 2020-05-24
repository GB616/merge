using System;
using System.Collections.Generic;
using System.Text;

namespace merge
{
     class DataCustomer
     {
        private String customer;
        private String product;
        private int invoice;
        private int quantity;
        private float price;
        private float cost;
        private float totalAmount;

        public DataCustomer()
        {
            customer = "none" ;
            product = "none";
            invoice = -1;
            quantity = -1;
            price = -1;
            cost = -1;
            totalAmount = -1;

        }

        public DataCustomer(DataCustomer d)
        {
            this.customer = d.customer;
            this.product = d.product;
            this.invoice = d.invoice;
            this.quantity = d.quantity;
            this.price = d.price;
            this.cost = d.cost;
            this.totalAmount = d.totalAmount;
        }

        public void setCustomer(String value )
        {
            this.customer = value.Trim();
        }

        public string getCustomer()
        {
            return this.customer;
        }

        public void setProduct(String value)
        {
            this.product = value.Trim();
        }

        public string getProduct()
        {
            return this.product;
        }

        public void setQuantity(int value)
        {
            this.quantity = value;
        }

        public int getQuantity()
        {
            return this.quantity;
        }
        public void setInvoice(int value)
        {
            this.invoice = value;
        }

        public int getInvoice()
        {
            return this.invoice;
        }
        
        public void setPrice(float value)
        {
            this.price = value;
        }

        public float getPrice()
        {
            return this.price;
        }

        public void setCost(float value)
        {
            this.cost = value;
        }

        public float getCost()
        {
            return this.cost;
        }

        public void setAmount(float value)
        {
            this.totalAmount = value;
        }

        public float getAmount()
        {
           return this.totalAmount;
        }
        


    }
}
