using System;
using System.ServiceModel;

namespace LComplete.Framework.Wcf
{
    public class WcfClientManager<TChannel> : IDisposable where TChannel : class
    {
        private TChannel _channel;

        private ChannelFactory<TChannel> _factory;

        public TChannel Channel
        {
            get
            {
                if (_channel == null)
                {
                    _factory = new ChannelFactory<TChannel>(typeof(TChannel).Name);
                    _channel = _factory.CreateChannel();
                }

                return _channel;
            }
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                try
                {
                    ((IClientChannel)_channel).Close();
                }
                catch
                {
                    ((IClientChannel)_channel).Abort();
                }
                try
                {
                    _factory.Close();
                }
                catch
                {
                    _factory.Abort();
                }

                _channel = null;
                _factory = null;
            }
        }
    }
}
