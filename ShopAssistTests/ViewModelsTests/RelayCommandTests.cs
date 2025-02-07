using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.ViewModels;
using System;

namespace ShopAssist.ViewModels.Tests
{
    [TestClass()]
    public class RelayCommandTests
    {
        [TestMethod()]
        public void RelayCommandTest()
        {
            Action<object> action = (a) => { };
            RelayCommand cmd = new RelayCommand(action);

            PrivateObject privCmd = new PrivateObject(cmd);
            Assert.IsNotNull(privCmd.GetField("execute"));
        }

        [TestMethod()]
        public void CanExecuteTest()
        {
            Action<object> action = (a) => { };
            Func<object, bool> returnsTrue = (a) => true;
            Func<object, bool> returnsFalse = (a) => false;
            RelayCommand cmd1 = new RelayCommand(action, returnsTrue);
            RelayCommand cmd2 = new RelayCommand(action, returnsFalse);
            Assert.IsTrue(cmd1.CanExecute());
            Assert.IsFalse(cmd2.CanExecute());
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            bool didExecute = false;
            Action<object> action = (a) => { didExecute = true; };
            RelayCommand cmd = new RelayCommand(action);
            cmd.Execute();
            Assert.IsTrue(didExecute);
        }
    }
}