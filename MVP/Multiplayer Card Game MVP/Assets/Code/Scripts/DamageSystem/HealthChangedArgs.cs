namespace DamageSystem
{
    public readonly struct HealthChangedArgs
    {
        public readonly int OldHealth;
        public readonly int NewHealth;
        public readonly int HealthDifference;


        public HealthChangedArgs(int oldHealth, int newHealth)
        {
            OldHealth = oldHealth;
            NewHealth = newHealth;
            HealthDifference = newHealth - oldHealth;
        }
    }
}