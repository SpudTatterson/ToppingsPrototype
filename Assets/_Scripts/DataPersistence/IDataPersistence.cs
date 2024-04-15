

public interface IDataPersistence 
{
    public void SaveData(ref SettingsData data);
    public void LoadData(SettingsData data);
}

public interface ISettingDataPersistence : IDataPersistence
{

}
