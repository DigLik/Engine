namespace Engine.Kits.Default.Storage;

internal interface IComponentStorage
{
    bool Remove(uint entityId);
    bool Has(uint entityId);
}