public class RedPotion : Item
{
    public int healthRecoverAmount;

    public override void Use()
    {
        base.Use();
        status.RecoverHealth(healthRecoverAmount);
    }
}
