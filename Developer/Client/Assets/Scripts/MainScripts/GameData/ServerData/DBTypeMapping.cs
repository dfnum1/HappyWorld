//auto generator
namespace TopGame.Data
{
	public class DBTypeMapping 
	{
		public static int GetTypeIndex(System.Type type)
		{
			if(typeof(TopGame.SvrData.BaseDB) == type) return (int)TopGame.Data.EDBType.BaseInfo;
			return -1;
		}
		public static System.Type GetDBType( EDBType type)
		{
			switch(type)
			{
				case TopGame.Data.EDBType.BaseInfo: return typeof(TopGame.SvrData.BaseDB);
			}
			return null;
		}
		public static AProxyDB NewProxyDB(EDBType type, SvrData.User user)
		{
			AProxyDB proxy = null;
			switch(type)
			{
				case TopGame.Data.EDBType.BaseInfo: proxy = new TopGame.SvrData.BaseDB(); break;
			}
			if(proxy!=null) proxy.Init(user);
			return proxy;
		}
	}
}
