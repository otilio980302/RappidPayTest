namespace RappidPayTest.UnitTest_Main
{
    [TestClass]
    public class ConexionTest
    {
        [TestMethod]
        public void CheckingConexion()
        {
            var dbCnResult = Configurations.DbConexionStringConfig.DbConexion().Database.CanConnect();

            Assert.IsTrue(dbCnResult, "Error trying to connect to de db using de CnString in Configuration/DbConexionStringConfig");

        }
    }
}