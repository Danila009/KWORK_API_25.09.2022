using System;

namespace FilmsApi.Model
{
    public class Subscription
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int ShopId { get; set; }
        public Boolean MainSubscription { get; set; }
    }
}
