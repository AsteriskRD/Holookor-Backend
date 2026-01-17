namespace HolookorBackend.Core.Domain.Entities
{
    public class PricingConfig : AuditableEntities
    {
        public decimal ServiceFee { get; private set; }
        public decimal TaxPercentage { get; private set; }
        public bool IsActive { get; private set; }

        private PricingConfig() { }

        public PricingConfig(decimal serviceFee, decimal taxPercentage)
        {
            ServiceFee = serviceFee;
            TaxPercentage = taxPercentage;
            IsActive = true;
        }
    }

}
