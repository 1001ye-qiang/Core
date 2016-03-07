
public interface IConnectManager
{
    void addService(int id, IService service);
    IService getService(int id);
}