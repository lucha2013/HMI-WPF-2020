using System;
using System.Collections.Generic;
using System.Text;

namespace DataServer
{
    public abstract class ITag : IComparable<ITag>
    {
        protected short _id;
        public short ID
        {
            get
            {
                return _id;
            }
        }

        protected bool _active = true;
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _group.SetActiveState(value, _id);
                _active = value;
            }
        }

        protected QUALITIES _quality;
        public QUALITIES Quality
        {
            get
            {
                return _quality;
            }
        }

        protected Storage _value;
        public Storage Value
        {
            get
            {
                return _value;
            }
        }

        protected DateTime _timeStamp = DateTime.MinValue;
        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }
        }

        protected DeviceAddress _plcAddress;
        public DeviceAddress Address
        {
            get
            {
                return _plcAddress;
            }
            set
            {
                _plcAddress = value;
            }
        }

        protected IGroup _group;
        public IGroup Parent
        {
            get
            {
                return _group;
            }
        }

        protected ITag(short id, DeviceAddress address, IGroup group)
        {
            _id = id;
            _group = group;
            _plcAddress = address;
        }

        public void Update(Storage newvalue, DateTime timeStamp, QUALITIES quality)
        {
            if (_timeStamp > timeStamp) return;//如果时间戳更旧或值未改变
            if (ValueChanging != null)
            {
                ValueChanging(this, new ValueChangingEventArgs<Storage>(quality, _value, newvalue, _timeStamp, timeStamp));
            }
            _timeStamp = timeStamp;
            _quality = quality;
            if (quality == QUALITIES.QUALITY_GOOD)
            {
                _value = newvalue;
                if (ValueChanged != null)
                {
                    ValueChanged(this, new ValueChangedEventArgs(_value));
                }
            }
        }

        public abstract bool Refresh(DataSource source = DataSource.Device);

        public abstract Storage Read(DataSource source = DataSource.Cache);

        protected abstract int InnerWrite(Storage value);

        public abstract int Write(object value);

        public int Write(Storage value, bool bForce)
        {
            DateTime time = DateTime.Now;
            _timeStamp = time;
            if (bForce)
            {
                if (ValueChanging != null)
                {
                    ValueChanging(this, new ValueChangingEventArgs<Storage>(QUALITIES.QUALITY_GOOD, _value, value, _timeStamp, time));
                }
            }
            int result = InnerWrite(value);
            if (bForce || result != 0)
            {
                var data = Read(DataSource.Device);
                if (data != value)
                {
                    time = DateTime.Now;
                    if (ValueChanging != null)
                    {
                        ValueChanging(this, new ValueChangingEventArgs<Storage>(QUALITIES.QUALITY_GOOD, _value, data, _timeStamp, time));
                    }
                    _value = data;
                    _timeStamp = time;
                    return result;
                }
            }
            return 0;
        }

        public ValueChangingEventHandler<Storage> ValueChanging;

        public ValueChangedEventHandler ValueChanged;

        #region IComparable<PLCAddress> Members

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public int CompareTo(ITag other)
        {
            return _plcAddress.CompareTo(other._plcAddress);
        }

        #endregion
    }
}
