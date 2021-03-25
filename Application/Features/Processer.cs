using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public abstract class Processer
    {
        private bool completeInitalization = true;
        private bool completeExecution = true;

        public void CheckAndProcess()
        {
            DoInitializationChecks();

            CheckAndInitalize();

            DoExecutionChecks();

            CheckAndExecute();
        }

        private void CheckAndInitalize()
        {
            if (completeInitalization)
                Initalize();
        }

        private void CheckAndExecute()
        {
            if (completeExecution)
                Execute();
        }

        protected abstract void DoInitializationChecks();

        protected abstract void Initalize();

        protected abstract void DoExecutionChecks();

        protected abstract void Execute();
    }
}
