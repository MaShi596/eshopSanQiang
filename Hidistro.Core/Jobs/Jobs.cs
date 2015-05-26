using Hidistro.Core.Configuration;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Xml;
namespace Hidistro.Core.Jobs
{
	public class Jobs : IDisposable
	{
		private static readonly Jobs _jobs;
		private static int _instancesOfParent;
		private Hashtable jobList = new Hashtable();
		private int Interval = 900000;
		private Timer singleTimer = null;
		private DateTime _created;
		private DateTime _started;
		private DateTime _completed;
		private bool _isRunning;
		private bool disposed = false;
		public Hashtable CurrentJobs
		{
			get
			{
				return this.jobList;
			}
		}
		public ListDictionary CurrentStats
		{
			get
			{
				return new ListDictionary
				{

					{
						"Created",
						this._created
					},

					{
						"LastStart",
						this._started
					},

					{
						"LastStop",
						this._completed
					},

					{
						"IsRunning",
						this._isRunning
					},

					{
						"Minutes",
						this.Interval / 60000
					}
				};
			}
		}
		static Jobs()
		{
			Jobs._jobs = null;
			Jobs._instancesOfParent = 0;
			Jobs._jobs = new Jobs();
		}
		public static Jobs Instance()
		{
			return Jobs._jobs;
		}
		private Jobs()
		{
			this._created = DateTime.Now;
		}
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "Created: {0}, LastStart: {1}, LastStop: {2}, IsRunning: {3}, Minutes: {4}", new object[]
			{
				this._created,
				this._started,
				this._completed,
				this._isRunning,
				this.Interval / 60000
			});
		}
		public void Start()
		{
			Interlocked.Increment(ref Jobs._instancesOfParent);
			lock (this.jobList.SyncRoot)
			{
				if (this.jobList.Count == 0)
				{
					HiConfiguration config = HiConfiguration.GetConfig();
					XmlNode configSection = config.GetConfigSection("Hishop/Jobs");
					bool flag2 = true;
					XmlAttribute xmlAttribute = configSection.Attributes["singleThread"];
					if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value) && string.Compare(xmlAttribute.Value, "false", true, CultureInfo.InvariantCulture) == 0)
					{
						flag2 = false;
					}
					XmlAttribute xmlAttribute2 = configSection.Attributes["minutes"];
					if (xmlAttribute2 != null && !string.IsNullOrEmpty(xmlAttribute2.Value))
					{
						int num = 1;
						if (int.TryParse(xmlAttribute2.Value, out num))
						{
							this.Interval = num * 60000;
						}
					}
					foreach (XmlNode xmlNode in configSection.ChildNodes)
					{
						if (configSection.NodeType != XmlNodeType.Comment && xmlNode.NodeType != XmlNodeType.Comment)
						{
							XmlAttribute xmlAttribute3 = xmlNode.Attributes["type"];
							XmlAttribute xmlAttribute4 = xmlNode.Attributes["name"];
							Type type = Type.GetType(xmlAttribute3.Value);
							if (type != null && !this.jobList.Contains(xmlAttribute4.Value))
							{
								Job job = new Job(type, xmlNode);
								if (flag2 && job.SingleThreaded)
								{
									job.InitializeTimer();
								}
								else
								{
									this.jobList[xmlAttribute4.Value] = job;
								}
							}
						}
					}
					if (this.jobList.Count > 0)
					{
						this.singleTimer = new Timer(new TimerCallback(this.call_back), null, this.Interval, this.Interval);
					}
				}
			}
		}
		private void call_back(object state)
		{
			this._isRunning = true;
			this._started = DateTime.Now;
			this.singleTimer.Change(-1, -1);
			foreach (Job job in this.jobList.Values)
			{
				if (job.Enabled)
				{
					job.ExecuteJob();
				}
			}
			this.singleTimer.Change(this.Interval, this.Interval);
			this._isRunning = false;
			this._completed = DateTime.Now;
		}
		public void Stop()
		{
			Interlocked.Decrement(ref Jobs._instancesOfParent);
			if (Jobs._instancesOfParent <= 0 && this.jobList != null)
			{
				lock (this.jobList.SyncRoot)
				{
					foreach (Job job in this.jobList.Values)
					{
						job.Dispose();
					}
					this.jobList.Clear();
					if (this.singleTimer != null)
					{
						this.singleTimer.Dispose();
						this.singleTimer = null;
					}
				}
			}
		}
		public bool IsJobEnabled(string jobName)
		{
			return this.jobList.Contains(jobName) && ((Job)this.jobList[jobName]).Enabled;
		}
		public void Dispose()
		{
			if (this.singleTimer != null && !this.disposed)
			{
				bool flag = false;
				try
				{
					Monitor.Enter(this, ref flag);
					this.singleTimer.Dispose();
					this.singleTimer = null;
					this.disposed = true;
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
			}
		}
	}
}
