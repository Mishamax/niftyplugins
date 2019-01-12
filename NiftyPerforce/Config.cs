// Copyright (C) 2006-2010 Jim Tilander. See COPYING for and README for more details.
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Aurora
{
	namespace NiftyPerforce
	{
		[Serializable]
		public class Config
		{
			private bool mDirty = false;
			private bool mEnableBindings = false;
            private bool mEnableContextMenus = false;
            private bool mEnableAdvancedCommands = false;
            private bool m_autoCheckoutOnEdit = false;
			private bool m_autoCheckoutProject = false;
			private bool m_autoCheckoutOnSave = false;
			private bool m_autoCheckoutOnBuild = false;
			private bool m_autoAdd = false;
			private bool m_autoDelete = false;
			private bool m_useSystemConnection = true;
			private bool m_ignoreReadOnlyOnEdit = false;
			//private bool m_warnOnEditNewerFile = false;
            private bool m_preferFoolVisualClient = false;      // <<--- use p4v instead of p4win
			private string m_port = "";
			private string m_client = "";
			private string m_username = "";
			private string m_mainlinePath = "";

			[XmlIgnore]
			public string mFileName = "";

			[XmlIgnore]
			[BrowsableAttribute(false)]
			public bool Dirty
			{
				get { return mDirty; }
			}

            [Category("General"), DisplayName("Enable key bindings"), Description("Enable key bindings.")]
			public bool EnableBindings
			{
				get { return mEnableBindings; }
                set { mEnableBindings = value; mDirty = true; }
			}

            [Category("General"), DisplayName("Enable context menus"), Description("Enable context menus.")]
            public bool EnableContextMenus
            {
                get { return mEnableContextMenus; }
                set { mEnableContextMenus = value; mDirty = true; }
            }

            [Category("General"), DisplayName("Enable advanced commands"), Description("Enable advanced commands (open all modified files for edit, commands on mainline branch).")]
            public bool EnableAdvancedCommands
            {
                get { return mEnableAdvancedCommands; }
                set { mEnableAdvancedCommands = value; mDirty = true; }
            }

            [Category("Operation"), DisplayName("Auto checkout on edit"), Description("Controls if we automagically check out files from perforce upon keypress (loose some performance in editor).")]
			public bool autoCheckoutOnEdit
			{
				get { return m_autoCheckoutOnEdit; }
				set { m_autoCheckoutOnEdit = value; mDirty = true; }
			}

			[Category("Operation"), DisplayName("Auto checkout project"), Description("Automatically check out projects on edit properties (loose some performance in editor).")]
			public bool autoCheckoutProject
			{
				get { return m_autoCheckoutProject; }
				set { m_autoCheckoutProject = value; mDirty = true; }
			}

			[Category("Operation"), DisplayName("Auto checkout on save"), Description("Controls if we automagically check out files from perforce before saving.")]
			public bool autoCheckoutOnSave
			{
				get { return m_autoCheckoutOnSave; }
				set { m_autoCheckoutOnSave = value; mDirty = true; }
			}

			[Category("Operation"), DisplayName("Auto checkout on build"), Description("Automagically check out files when building (loose some performance in editor).")]
			public bool autoCheckoutOnBuild
			{
				get { return m_autoCheckoutOnBuild; }
				set { m_autoCheckoutOnBuild = value; mDirty = true; }
			}

			[Category("Operation"), DisplayName("Auto add to Perforce"), Description("Automagically add files to perforce.")]
			public bool autoAdd
			{
				get { return m_autoAdd; }
				set { m_autoAdd = value; mDirty = true; }
			}

			[Category("Operation"), DisplayName("Auto delete from Perforce"), Description("Automagically delete files from perforce when we're deleting files from visual studio (fairly dangerous).")]
			public bool autoDelete
			{
				get { return m_autoDelete; }
				set { m_autoDelete = value; mDirty = true; }
			}

			[Category("Operation"), DisplayName("Ignore read-only flag on edit"), Description("Try to do a p4 edit even though the file is writable. Useful if you have a git repository above your p4 workspace. Costly!")]
			public bool ignoreReadOnlyOnEdit
			{
				get { return m_ignoreReadOnlyOnEdit; }
				set { m_ignoreReadOnlyOnEdit = value; mDirty = true; }
			}

			/*[Category("Operation"), Description("Throw up a dialog box if you try to edit a file that has a newer version in the repository.")]
			public bool warnOnEditNewerFile
			{
				get { return m_warnOnEditNewerFile; }
				set { m_warnOnEditNewerFile = value; mDirty = true; }
			}*/

            [Category("Operation"), DisplayName("Prefer P4V to P4Win"), Description("Use p4v even if superior p4win is found.")]
            public bool preferVisualClient
            {
                get { return m_preferFoolVisualClient; }
                set { m_preferFoolVisualClient = value; mDirty = true; }
            }

			[Category("Connection"), DisplayName("Use system settings"), Description("Use config from system. Effectively disables the settings inside this dialog for the client etc and picks up the settings from the registry/p4config environment.")]
			public bool useSystemEnv
			{
				get { return m_useSystemConnection; }
				set { m_useSystemConnection = value; mDirty = true; }
			}

			[Category("Connection"), DisplayName("Port"), Description("Perforce port number.")]
			public string port
			{
				get { return m_port; }
				set { m_port = value; mDirty = true; }
			}

			[Category("Connection"), DisplayName("Client"), Description("Perforce client.")]
			public string client
			{
				get { return m_client; }
				set { m_client = value; mDirty = true; }
			}

			[Category("Connection"), DisplayName("Username"), Description("Perforce username.")]
			public string username
			{
				get { return m_username; }
				set { m_username = value; mDirty = true; }
			}

			[Category("Branching"), DisplayName("Mainline path"), Description("Where we can find the mainline version of this file.")]
			public string MainLinePath
			{
				get { return m_mainlinePath; }
				set { m_mainlinePath = value; mDirty = true; }
			}

			public static Config Load(string filename)
			{
				Config o = null;
				if(System.IO.File.Exists(filename))
				{
					Log.Info("Loading configuration from {0}", filename);
					try 
					{
						o = File.LoadXML<Config>(filename);
					}
					catch( InvalidOperationException )
					{
						Log.Error("Failed to load configuration from {0}, reverting to default config.", filename);
					}
				}
				if( o == null)
				{
					o = new Config();
				}
				o.mFileName = filename;
				Singleton<Config>.Instance = o;
				return o;
			}

			public void Save()
			{
				if(mDirty)
				{
					Log.Info("Saving configuration to {0}", mFileName);
					File.SaveXML<Config>(mFileName, this);
					mDirty = false;
					Singleton<Config>.Instance = this;
				}
			}
		}
	}
}
