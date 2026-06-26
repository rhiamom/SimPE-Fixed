/***************************************************************************
 *   Copyright (C) 2006-2007 by JFade / DJS Sims                            *
 *   (The Sims Programming Group)                                           *
 *                                                                          *
 *   Originally written in VB.NET for Sims 2 Collection Creator.            *
 *   Decompiled with ILSpy 2026-06-26 and included in this repository as    *
 *   reference for the SimPE Tool-plugin port.                              *
 *                                                                          *
 *   Used by permission of the original author (granted 2026-06-26).        *
 *   Reference only — not part of the SimPE-Fixed build, not relicensed.    *
 ***************************************************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using CCB;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace CollectionCreator;

public class frmMain : Form
{
	[AccessedThroughProperty("cmdMoveUp")]
	private Button _cmdMoveUp;

	[AccessedThroughProperty("txtCollID")]
	private TextBox _txtCollID;

	[AccessedThroughProperty("lstDatFileNames")]
	private ListBox _lstDatFileNames;

	[AccessedThroughProperty("txtImgPath")]
	private TextBox _txtImgPath;

	[AccessedThroughProperty("cmdOptions")]
	private Button _cmdOptions;

	[AccessedThroughProperty("cmdAbout")]
	private Button _cmdAbout;

	[AccessedThroughProperty("txtThumbDir")]
	private TextBox _txtThumbDir;

	[AccessedThroughProperty("PictureBox2")]
	private PictureBox _PictureBox2;

	[AccessedThroughProperty("cmdFindThumbDir")]
	private Button _cmdFindThumbDir;

	[AccessedThroughProperty("txtCollDir")]
	private TextBox _txtCollDir;

	[AccessedThroughProperty("cmdFindCollDir")]
	private Button _cmdFindCollDir;

	[AccessedThroughProperty("Label15")]
	private Label _Label15;

	[AccessedThroughProperty("lstBatchFileList")]
	private ListBox _lstBatchFileList;

	[AccessedThroughProperty("chkCompression")]
	private CheckBox _chkCompression;

	[AccessedThroughProperty("Panel1")]
	private StatusBarPanel _Panel1;

	[AccessedThroughProperty("StatusBar1")]
	private StatusBar _StatusBar1;

	[AccessedThroughProperty("lstBatchCategories")]
	private ListBox _lstBatchCategories;

	[AccessedThroughProperty("lblBatchAddTotal")]
	private Label _lblBatchAddTotal;

	[AccessedThroughProperty("lstCategories")]
	private ListBox _lstCategories;

	[AccessedThroughProperty("PictureBox1")]
	private PictureBox _PictureBox1;

	[AccessedThroughProperty("txtCollName")]
	private TextBox _txtCollName;

	[AccessedThroughProperty("lstInstance2")]
	private ListBox _lstInstance2;

	[AccessedThroughProperty("GroupBox4")]
	private GroupBox _GroupBox4;

	[AccessedThroughProperty("lblProgressDesc")]
	private Label _lblProgressDesc;

	[AccessedThroughProperty("ProgressBar1")]
	private ProgressBar _ProgressBar1;

	[AccessedThroughProperty("Label14")]
	private Label _Label14;

	[AccessedThroughProperty("lblProgress")]
	private Label _lblProgress;

	[AccessedThroughProperty("GroupBox3")]
	private GroupBox _GroupBox3;

	[AccessedThroughProperty("ToolTip1")]
	private ToolTip _ToolTip1;

	[AccessedThroughProperty("cdlOpenFile")]
	private OpenFileDialog _cdlOpenFile;

	[AccessedThroughProperty("FolderBrowserDialog1")]
	private FolderBrowserDialog _FolderBrowserDialog1;

	[AccessedThroughProperty("chkWarningOff")]
	private CheckBox _chkWarningOff;

	[AccessedThroughProperty("cmdCloseOptions")]
	private Button _cmdCloseOptions;

	[AccessedThroughProperty("cdlSaveFile")]
	private SaveFileDialog _cdlSaveFile;

	[AccessedThroughProperty("lstRecursive")]
	private ListBox _lstRecursive;

	[AccessedThroughProperty("Label13")]
	private Label _Label13;

	[AccessedThroughProperty("GroupBox1")]
	private GroupBox _GroupBox1;

	[AccessedThroughProperty("lstABC1")]
	private ListBox _lstABC1;

	[AccessedThroughProperty("Label12")]
	private Label _Label12;

	[AccessedThroughProperty("lstListOfItems")]
	private ListBox _lstListOfItems;

	[AccessedThroughProperty("cmdAlphaSort")]
	private Button _cmdAlphaSort;

	[AccessedThroughProperty("txtCTSSName")]
	private TextBox _txtCTSSName;

	[AccessedThroughProperty("Label11")]
	private Label _Label11;

	[AccessedThroughProperty("lstBatchAdd")]
	private ListBox _lstBatchAdd;

	[AccessedThroughProperty("txtCTSSDesc")]
	private TextBox _txtCTSSDesc;

	[AccessedThroughProperty("Label1")]
	private Label _Label1;

	[AccessedThroughProperty("cmdBatchAddRemove")]
	private Button _cmdBatchAddRemove;

	[AccessedThroughProperty("Label2")]
	private Label _Label2;

	[AccessedThroughProperty("txtGUID")]
	private TextBox _txtGUID;

	[AccessedThroughProperty("cmdBatchAddDown")]
	private Button _cmdBatchAddDown;

	[AccessedThroughProperty("txtFileName")]
	private TextBox _txtFileName;

	[AccessedThroughProperty("Label3")]
	private Label _Label3;

	[AccessedThroughProperty("txtGroup")]
	private TextBox _txtGroup;

	[AccessedThroughProperty("cmdAddItem")]
	private Button _cmdAddItem;

	[AccessedThroughProperty("cmdCancel")]
	private Button _cmdCancel;

	[AccessedThroughProperty("cmdBatchAddUp")]
	private Button _cmdBatchAddUp;

	[AccessedThroughProperty("Command1")]
	private Button _Command1;

	[AccessedThroughProperty("lstInstance")]
	private ListBox _lstInstance;

	[AccessedThroughProperty("cmdBatchAdd")]
	private Button _cmdBatchAdd;

	[AccessedThroughProperty("cmdAddWallFloor")]
	private Button _cmdAddWallFloor;

	[AccessedThroughProperty("cmdExit")]
	private Button _cmdExit;

	[AccessedThroughProperty("cmdSaveColl")]
	private Button _cmdSaveColl;

	[AccessedThroughProperty("cmdMakeNewColl")]
	private Button _cmdMakeNewColl;

	[AccessedThroughProperty("cmdEditColl")]
	private Button _cmdEditColl;

	[AccessedThroughProperty("cmdBackUpColl")]
	private Button _cmdBackUpColl;

	[AccessedThroughProperty("Label4")]
	private Label _Label4;

	[AccessedThroughProperty("cmdFinishBatchAdd")]
	private Button _cmdFinishBatchAdd;

	[AccessedThroughProperty("Label5")]
	private Label _Label5;

	[AccessedThroughProperty("txtPackagePath")]
	private TextBox _txtPackagePath;

	[AccessedThroughProperty("cmdCancelBatchAdd")]
	private Button _cmdCancelBatchAdd;

	[AccessedThroughProperty("Label6")]
	private Label _Label6;

	[AccessedThroughProperty("GroupBox2")]
	private GroupBox _GroupBox2;

	[AccessedThroughProperty("cmdRemoveItem")]
	private Button _cmdRemoveItem;

	[AccessedThroughProperty("cmdMoveDown")]
	private Button _cmdMoveDown;

	[AccessedThroughProperty("Picture1")]
	private PictureBox _Picture1;

	[AccessedThroughProperty("Label7")]
	private Label _Label7;

	[AccessedThroughProperty("btnDEBUG")]
	private Button _btnDEBUG;

	[AccessedThroughProperty("cmdLoadPic")]
	private Button _cmdLoadPic;

	[AccessedThroughProperty("cmbCollType")]
	private ComboBox _cmbCollType;

	[AccessedThroughProperty("lstOffset")]
	private ListBox _lstOffset;

	[AccessedThroughProperty("lstSize")]
	private ListBox _lstSize;

	[AccessedThroughProperty("txtResourceCount")]
	private TextBox _txtResourceCount;

	[AccessedThroughProperty("Label8")]
	private Label _Label8;

	[AccessedThroughProperty("Label9")]
	private Label _Label9;

	[AccessedThroughProperty("txtIndexSize")]
	private TextBox _txtIndexSize;

	[AccessedThroughProperty("Label10")]
	private Label _Label10;

	[AccessedThroughProperty("lstGroups")]
	private ListBox _lstGroups;

	[AccessedThroughProperty("lstResources")]
	private ListBox _lstResources;

	private IContainer components;

	public static string OldStaText = "";

	public static int ThumbPackageLoaded = 0;

	internal virtual Button btnDEBUG
	{
		get
		{
			return _btnDEBUG;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_btnDEBUG != null)
			{
				((Control)_btnDEBUG).Click -= btnDEBUG_Click;
			}
			_btnDEBUG = value;
			if (_btnDEBUG != null)
			{
				((Control)_btnDEBUG).Click += btnDEBUG_Click;
			}
		}
	}

	internal virtual GroupBox GroupBox2
	{
		get
		{
			return _GroupBox2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_GroupBox2 != null)
			{
			}
			_GroupBox2 = value;
			if (_GroupBox2 == null)
			{
			}
		}
	}

	public virtual Button cmdCancelBatchAdd
	{
		get
		{
			return _cmdCancelBatchAdd;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdCancelBatchAdd != null)
			{
				((Control)_cmdCancelBatchAdd).MouseLeave -= cmdCancelBatchAdd_MouseLeave;
				((Control)_cmdCancelBatchAdd).MouseEnter -= cmdCancelBatchAdd_MouseEnter;
				((Control)_cmdCancelBatchAdd).Click -= cmdCancelBatchAdd_Click;
			}
			_cmdCancelBatchAdd = value;
			if (_cmdCancelBatchAdd != null)
			{
				((Control)_cmdCancelBatchAdd).MouseLeave += cmdCancelBatchAdd_MouseLeave;
				((Control)_cmdCancelBatchAdd).MouseEnter += cmdCancelBatchAdd_MouseEnter;
				((Control)_cmdCancelBatchAdd).Click += cmdCancelBatchAdd_Click;
			}
		}
	}

	public virtual Button cmdFinishBatchAdd
	{
		get
		{
			return _cmdFinishBatchAdd;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdFinishBatchAdd != null)
			{
				((Control)_cmdFinishBatchAdd).MouseLeave -= cmdFinishBatchAdd_MouseLeave;
				((Control)_cmdFinishBatchAdd).MouseEnter -= cmdFinishBatchAdd_MouseEnter;
				((Control)_cmdFinishBatchAdd).Click -= cmdFinishBatchAdd_Click;
			}
			_cmdFinishBatchAdd = value;
			if (_cmdFinishBatchAdd != null)
			{
				((Control)_cmdFinishBatchAdd).MouseLeave += cmdFinishBatchAdd_MouseLeave;
				((Control)_cmdFinishBatchAdd).MouseEnter += cmdFinishBatchAdd_MouseEnter;
				((Control)_cmdFinishBatchAdd).Click += cmdFinishBatchAdd_Click;
			}
		}
	}

	public virtual Button cmdBatchAddUp
	{
		get
		{
			return _cmdBatchAddUp;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdBatchAddUp != null)
			{
				((Control)_cmdBatchAddUp).MouseLeave -= cmdBatchAddUp_MouseLeave;
				((Control)_cmdBatchAddUp).MouseEnter -= cmdBatchAddUp_MouseEnter;
				((Control)_cmdBatchAddUp).Click -= cmdBatchAddUp_Click;
			}
			_cmdBatchAddUp = value;
			if (_cmdBatchAddUp != null)
			{
				((Control)_cmdBatchAddUp).MouseLeave += cmdBatchAddUp_MouseLeave;
				((Control)_cmdBatchAddUp).MouseEnter += cmdBatchAddUp_MouseEnter;
				((Control)_cmdBatchAddUp).Click += cmdBatchAddUp_Click;
			}
		}
	}

	public virtual Button cmdBatchAddDown
	{
		get
		{
			return _cmdBatchAddDown;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdBatchAddDown != null)
			{
				((Control)_cmdBatchAddDown).MouseLeave -= cmdBatchAddDown_MouseLeave;
				((Control)_cmdBatchAddDown).MouseEnter -= cmdBatchAddDown_MouseEnter;
				((Control)_cmdBatchAddDown).Click -= cmdBatchAddDown_Click;
			}
			_cmdBatchAddDown = value;
			if (_cmdBatchAddDown != null)
			{
				((Control)_cmdBatchAddDown).MouseLeave += cmdBatchAddDown_MouseLeave;
				((Control)_cmdBatchAddDown).MouseEnter += cmdBatchAddDown_MouseEnter;
				((Control)_cmdBatchAddDown).Click += cmdBatchAddDown_Click;
			}
		}
	}

	public virtual Button cmdBatchAddRemove
	{
		get
		{
			return _cmdBatchAddRemove;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdBatchAddRemove != null)
			{
				((Control)_cmdBatchAddRemove).MouseLeave -= cmdBatchAddRemove_MouseLeave;
				((Control)_cmdBatchAddRemove).MouseEnter -= cmdBatchAddRemove_MouseEnter;
				((Control)_cmdBatchAddRemove).Click -= cmdBatchAddRemove_Click;
			}
			_cmdBatchAddRemove = value;
			if (_cmdBatchAddRemove != null)
			{
				((Control)_cmdBatchAddRemove).MouseLeave += cmdBatchAddRemove_MouseLeave;
				((Control)_cmdBatchAddRemove).MouseEnter += cmdBatchAddRemove_MouseEnter;
				((Control)_cmdBatchAddRemove).Click += cmdBatchAddRemove_Click;
			}
		}
	}

	internal virtual ListBox lstBatchAdd
	{
		get
		{
			return _lstBatchAdd;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstBatchAdd != null)
			{
				_lstBatchAdd.SelectedIndexChanged -= lstBatchAdd_SelectedIndexChanged;
			}
			_lstBatchAdd = value;
			if (_lstBatchAdd != null)
			{
				_lstBatchAdd.SelectedIndexChanged += lstBatchAdd_SelectedIndexChanged;
			}
		}
	}

	public virtual Button cmdAlphaSort
	{
		get
		{
			return _cmdAlphaSort;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdAlphaSort != null)
			{
				((Control)_cmdAlphaSort).MouseLeave -= cmdAlphaSort_MouseLeave;
				((Control)_cmdAlphaSort).MouseEnter -= cmdAlphaSort_MouseEnter;
				((Control)_cmdAlphaSort).Click -= cmdAlphaSort_Click;
			}
			_cmdAlphaSort = value;
			if (_cmdAlphaSort != null)
			{
				((Control)_cmdAlphaSort).MouseLeave += cmdAlphaSort_MouseLeave;
				((Control)_cmdAlphaSort).MouseEnter += cmdAlphaSort_MouseEnter;
				((Control)_cmdAlphaSort).Click += cmdAlphaSort_Click;
			}
		}
	}

	internal virtual GroupBox GroupBox1
	{
		get
		{
			return _GroupBox1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_GroupBox1 != null)
			{
			}
			_GroupBox1 = value;
			if (_GroupBox1 == null)
			{
			}
		}
	}

	internal virtual Button cmdCloseOptions
	{
		get
		{
			return _cmdCloseOptions;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdCloseOptions != null)
			{
				((Control)_cmdCloseOptions).MouseLeave -= cmdCloseOptions_MouseLeave;
				((Control)_cmdCloseOptions).MouseEnter -= cmdCloseOptions_MouseEnter;
				((Control)_cmdCloseOptions).Click -= cmdCloseOptions_Click;
			}
			_cmdCloseOptions = value;
			if (_cmdCloseOptions != null)
			{
				((Control)_cmdCloseOptions).MouseLeave += cmdCloseOptions_MouseLeave;
				((Control)_cmdCloseOptions).MouseEnter += cmdCloseOptions_MouseEnter;
				((Control)_cmdCloseOptions).Click += cmdCloseOptions_Click;
			}
		}
	}

	internal virtual CheckBox chkWarningOff
	{
		get
		{
			return _chkWarningOff;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_chkWarningOff != null)
			{
			}
			_chkWarningOff = value;
			if (_chkWarningOff == null)
			{
			}
		}
	}

	internal virtual Label Label9
	{
		get
		{
			return _Label9;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label9 != null)
			{
			}
			_Label9 = value;
			if (_Label9 == null)
			{
			}
		}
	}

	internal virtual Button cmdFindCollDir
	{
		get
		{
			return _cmdFindCollDir;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdFindCollDir != null)
			{
				((Control)_cmdFindCollDir).MouseLeave -= cmdFindCollDir_MouseLeave;
				((Control)_cmdFindCollDir).MouseEnter -= cmdFindCollDir_MouseEnter;
				((Control)_cmdFindCollDir).Click -= cmdFindCollDir_Click;
			}
			_cmdFindCollDir = value;
			if (_cmdFindCollDir != null)
			{
				((Control)_cmdFindCollDir).MouseLeave += cmdFindCollDir_MouseLeave;
				((Control)_cmdFindCollDir).MouseEnter += cmdFindCollDir_MouseEnter;
				((Control)_cmdFindCollDir).Click += cmdFindCollDir_Click;
			}
		}
	}

	internal virtual Label Label8
	{
		get
		{
			return _Label8;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label8 != null)
			{
			}
			_Label8 = value;
			if (_Label8 == null)
			{
			}
		}
	}

	internal virtual TextBox txtCollDir
	{
		get
		{
			return _txtCollDir;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtCollDir != null)
			{
			}
			_txtCollDir = value;
			if (_txtCollDir == null)
			{
			}
		}
	}

	internal virtual Button cmdFindThumbDir
	{
		get
		{
			return _cmdFindThumbDir;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdFindThumbDir != null)
			{
				((Control)_cmdFindThumbDir).MouseLeave -= cmdFindThumbDir_MouseLeave;
				((Control)_cmdFindThumbDir).MouseEnter -= cmdFindThumbDir_MouseEnter;
				((Control)_cmdFindThumbDir).Click -= cmdFindThumbDir_Click;
			}
			_cmdFindThumbDir = value;
			if (_cmdFindThumbDir != null)
			{
				((Control)_cmdFindThumbDir).MouseLeave += cmdFindThumbDir_MouseLeave;
				((Control)_cmdFindThumbDir).MouseEnter += cmdFindThumbDir_MouseEnter;
				((Control)_cmdFindThumbDir).Click += cmdFindThumbDir_Click;
			}
		}
	}

	internal virtual TextBox txtThumbDir
	{
		get
		{
			return _txtThumbDir;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtThumbDir != null)
			{
			}
			_txtThumbDir = value;
			if (_txtThumbDir == null)
			{
			}
		}
	}

	internal virtual Label Label11
	{
		get
		{
			return _Label11;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label11 != null)
			{
			}
			_Label11 = value;
			if (_Label11 == null)
			{
			}
		}
	}

	internal virtual Label Label10
	{
		get
		{
			return _Label10;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label10 != null)
			{
			}
			_Label10 = value;
			if (_Label10 == null)
			{
			}
		}
	}

	public virtual Button cmdAbout
	{
		get
		{
			return _cmdAbout;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdAbout != null)
			{
				((Control)_cmdAbout).MouseEnter -= cmdAbout_MouseEnter;
				((Control)_cmdAbout).MouseLeave -= cmdAbout_MouseLeave;
				((Control)_cmdAbout).Click -= cmdAbout_Click;
			}
			_cmdAbout = value;
			if (_cmdAbout != null)
			{
				((Control)_cmdAbout).MouseEnter += cmdAbout_MouseEnter;
				((Control)_cmdAbout).MouseLeave += cmdAbout_MouseLeave;
				((Control)_cmdAbout).Click += cmdAbout_Click;
			}
		}
	}

	public virtual Button cmdOptions
	{
		get
		{
			return _cmdOptions;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdOptions != null)
			{
				((Control)_cmdOptions).MouseEnter -= cmdOptions_MouseEnter;
				((Control)_cmdOptions).MouseLeave -= cmdOptions_MouseLeave;
				((Control)_cmdOptions).Click -= cmdOptions_Click;
			}
			_cmdOptions = value;
			if (_cmdOptions != null)
			{
				((Control)_cmdOptions).MouseEnter += cmdOptions_MouseEnter;
				((Control)_cmdOptions).MouseLeave += cmdOptions_MouseLeave;
				((Control)_cmdOptions).Click += cmdOptions_Click;
			}
		}
	}

	internal virtual TextBox txtImgPath
	{
		get
		{
			return _txtImgPath;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtImgPath != null)
			{
			}
			_txtImgPath = value;
			if (_txtImgPath == null)
			{
			}
		}
	}

	internal virtual ListBox lstDatFileNames
	{
		get
		{
			return _lstDatFileNames;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstDatFileNames != null)
			{
			}
			_lstDatFileNames = value;
			if (_lstDatFileNames == null)
			{
			}
		}
	}

	internal virtual TextBox txtCollID
	{
		get
		{
			return _txtCollID;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtCollID != null)
			{
			}
			_txtCollID = value;
			if (_txtCollID == null)
			{
			}
		}
	}

	internal virtual TextBox txtPackagePath
	{
		get
		{
			return _txtPackagePath;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtPackagePath != null)
			{
			}
			_txtPackagePath = value;
			if (_txtPackagePath == null)
			{
			}
		}
	}

	internal virtual ListBox lstInstance2
	{
		get
		{
			return _lstInstance2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstInstance2 != null)
			{
				_lstInstance2.SelectedIndexChanged -= lstInstance2_SelectedIndexChanged;
			}
			_lstInstance2 = value;
			if (_lstInstance2 != null)
			{
				_lstInstance2.SelectedIndexChanged += lstInstance2_SelectedIndexChanged;
			}
		}
	}

	internal virtual ListBox lstInstance
	{
		get
		{
			return _lstInstance;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstInstance != null)
			{
				((Control)_lstInstance).SizeChanged -= lstInstance_SizeChanged;
				_lstInstance.SelectedIndexChanged -= lstInstance_SelectedIndexChanged;
			}
			_lstInstance = value;
			if (_lstInstance != null)
			{
				((Control)_lstInstance).SizeChanged += lstInstance_SizeChanged;
				_lstInstance.SelectedIndexChanged += lstInstance_SelectedIndexChanged;
			}
		}
	}

	internal virtual ListBox lstGroups
	{
		get
		{
			return _lstGroups;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstGroups != null)
			{
				_lstGroups.SelectedIndexChanged -= lstGroups_SelectedIndexChanged;
			}
			_lstGroups = value;
			if (_lstGroups != null)
			{
				_lstGroups.SelectedIndexChanged += lstGroups_SelectedIndexChanged;
			}
		}
	}

	internal virtual ListBox lstResources
	{
		get
		{
			return _lstResources;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstResources != null)
			{
				_lstResources.SelectedIndexChanged -= lstResources_SelectedIndexChanged;
			}
			_lstResources = value;
			if (_lstResources != null)
			{
				_lstResources.SelectedIndexChanged += lstResources_SelectedIndexChanged;
			}
		}
	}

	internal virtual TextBox txtIndexSize
	{
		get
		{
			return _txtIndexSize;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtIndexSize != null)
			{
			}
			_txtIndexSize = value;
			if (_txtIndexSize == null)
			{
			}
		}
	}

	internal virtual TextBox txtResourceCount
	{
		get
		{
			return _txtResourceCount;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtResourceCount != null)
			{
			}
			_txtResourceCount = value;
			if (_txtResourceCount == null)
			{
			}
		}
	}

	public virtual ListBox lstSize
	{
		get
		{
			return _lstSize;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstSize != null)
			{
				_lstSize.SelectedIndexChanged -= lstSize_SelectedIndexChanged;
			}
			_lstSize = value;
			if (_lstSize != null)
			{
				_lstSize.SelectedIndexChanged += lstSize_SelectedIndexChanged;
			}
		}
	}

	public virtual ListBox lstOffset
	{
		get
		{
			return _lstOffset;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstOffset != null)
			{
				_lstOffset.SelectedIndexChanged -= lstOffset_SelectedIndexChanged;
			}
			_lstOffset = value;
			if (_lstOffset != null)
			{
				_lstOffset.SelectedIndexChanged += lstOffset_SelectedIndexChanged;
			}
		}
	}

	public virtual ComboBox cmbCollType
	{
		get
		{
			return _cmbCollType;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmbCollType != null)
			{
			}
			_cmbCollType = value;
			if (_cmbCollType == null)
			{
			}
		}
	}

	public virtual Button cmdLoadPic
	{
		get
		{
			return _cmdLoadPic;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdLoadPic != null)
			{
				((Control)_cmdLoadPic).MouseLeave -= cmdLoadPic_MouseLeave;
				((Control)_cmdLoadPic).MouseEnter -= cmdLoadPic_MouseEnter;
				((Control)_cmdLoadPic).Click -= cmdLoadPic_Click;
			}
			_cmdLoadPic = value;
			if (_cmdLoadPic != null)
			{
				((Control)_cmdLoadPic).MouseLeave += cmdLoadPic_MouseLeave;
				((Control)_cmdLoadPic).MouseEnter += cmdLoadPic_MouseEnter;
				((Control)_cmdLoadPic).Click += cmdLoadPic_Click;
			}
		}
	}

	public virtual PictureBox Picture1
	{
		get
		{
			return _Picture1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Picture1 != null)
			{
			}
			_Picture1 = value;
			if (_Picture1 == null)
			{
			}
		}
	}

	public virtual Button cmdMoveDown
	{
		get
		{
			return _cmdMoveDown;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdMoveDown != null)
			{
				((Control)_cmdMoveDown).MouseLeave -= cmdMoveDown_MouseLeave;
				((Control)_cmdMoveDown).MouseEnter -= cmdMoveDown_MouseEnter;
				((Control)_cmdMoveDown).Click -= cmdMoveDown_Click;
			}
			_cmdMoveDown = value;
			if (_cmdMoveDown != null)
			{
				((Control)_cmdMoveDown).MouseLeave += cmdMoveDown_MouseLeave;
				((Control)_cmdMoveDown).MouseEnter += cmdMoveDown_MouseEnter;
				((Control)_cmdMoveDown).Click += cmdMoveDown_Click;
			}
		}
	}

	public virtual Button cmdRemoveItem
	{
		get
		{
			return _cmdRemoveItem;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdRemoveItem != null)
			{
				((Control)_cmdRemoveItem).MouseLeave -= cmdRemoveItem_MouseLeave;
				((Control)_cmdRemoveItem).MouseEnter -= cmdRemoveItem_MouseEnter;
				((Control)_cmdRemoveItem).Click -= cmdRemoveItem_Click;
			}
			_cmdRemoveItem = value;
			if (_cmdRemoveItem != null)
			{
				((Control)_cmdRemoveItem).MouseLeave += cmdRemoveItem_MouseLeave;
				((Control)_cmdRemoveItem).MouseEnter += cmdRemoveItem_MouseEnter;
				((Control)_cmdRemoveItem).Click += cmdRemoveItem_Click;
			}
		}
	}

	public virtual Button cmdMoveUp
	{
		get
		{
			return _cmdMoveUp;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdMoveUp != null)
			{
				((Control)_cmdMoveUp).MouseLeave -= cmdMoveUp_MouseLeave;
				((Control)_cmdMoveUp).MouseEnter -= cmdMoveUp_MouseEnter;
				((Control)_cmdMoveUp).Click -= cmdMoveUp_Click;
			}
			_cmdMoveUp = value;
			if (_cmdMoveUp != null)
			{
				((Control)_cmdMoveUp).MouseLeave += cmdMoveUp_MouseLeave;
				((Control)_cmdMoveUp).MouseEnter += cmdMoveUp_MouseEnter;
				((Control)_cmdMoveUp).Click += cmdMoveUp_Click;
			}
		}
	}

	public virtual Button cmdBackUpColl
	{
		get
		{
			return _cmdBackUpColl;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdBackUpColl != null)
			{
				((Control)_cmdBackUpColl).MouseLeave -= cmdBackUpColl_MouseLeave;
				((Control)_cmdBackUpColl).MouseEnter -= cmdBackUpColl_MouseEnter;
				((Control)_cmdBackUpColl).Click -= cmdBackUpColl_Click;
			}
			_cmdBackUpColl = value;
			if (_cmdBackUpColl != null)
			{
				((Control)_cmdBackUpColl).MouseLeave += cmdBackUpColl_MouseLeave;
				((Control)_cmdBackUpColl).MouseEnter += cmdBackUpColl_MouseEnter;
				((Control)_cmdBackUpColl).Click += cmdBackUpColl_Click;
			}
		}
	}

	public virtual Button cmdEditColl
	{
		get
		{
			return _cmdEditColl;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdEditColl != null)
			{
				((Control)_cmdEditColl).MouseLeave -= cmdEditColl_MouseLeave;
				((Control)_cmdEditColl).MouseEnter -= cmdEditColl_MouseEnter;
				((Control)_cmdEditColl).Click -= cmdEditColl_Click;
			}
			_cmdEditColl = value;
			if (_cmdEditColl != null)
			{
				((Control)_cmdEditColl).MouseLeave += cmdEditColl_MouseLeave;
				((Control)_cmdEditColl).MouseEnter += cmdEditColl_MouseEnter;
				((Control)_cmdEditColl).Click += cmdEditColl_Click;
			}
		}
	}

	public virtual Button cmdMakeNewColl
	{
		get
		{
			return _cmdMakeNewColl;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdMakeNewColl != null)
			{
				((Control)_cmdMakeNewColl).MouseLeave -= cmdMakeNewColl_MouseLeave;
				((Control)_cmdMakeNewColl).MouseEnter -= cmdMakeNewColl_MouseEnter;
				((Control)_cmdMakeNewColl).Click -= cmdMakeNewColl_Click;
			}
			_cmdMakeNewColl = value;
			if (_cmdMakeNewColl != null)
			{
				((Control)_cmdMakeNewColl).MouseLeave += cmdMakeNewColl_MouseLeave;
				((Control)_cmdMakeNewColl).MouseEnter += cmdMakeNewColl_MouseEnter;
				((Control)_cmdMakeNewColl).Click += cmdMakeNewColl_Click;
			}
		}
	}

	public virtual Button cmdSaveColl
	{
		get
		{
			return _cmdSaveColl;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdSaveColl != null)
			{
				((Control)_cmdSaveColl).MouseLeave -= cmdSaveColl_MouseLeave;
				((Control)_cmdSaveColl).MouseEnter -= cmdSaveColl_MouseEnter;
				((Control)_cmdSaveColl).Click -= cmdSaveColl_Click;
			}
			_cmdSaveColl = value;
			if (_cmdSaveColl != null)
			{
				((Control)_cmdSaveColl).MouseLeave += cmdSaveColl_MouseLeave;
				((Control)_cmdSaveColl).MouseEnter += cmdSaveColl_MouseEnter;
				((Control)_cmdSaveColl).Click += cmdSaveColl_Click;
			}
		}
	}

	public virtual Button cmdExit
	{
		get
		{
			return _cmdExit;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdExit != null)
			{
				((Control)_cmdExit).MouseLeave -= cmdExit_MouseLeave;
				((Control)_cmdExit).MouseEnter -= cmdExit_MouseEnter;
				((Control)_cmdExit).Click -= cmdExit_Click;
			}
			_cmdExit = value;
			if (_cmdExit != null)
			{
				((Control)_cmdExit).MouseLeave += cmdExit_MouseLeave;
				((Control)_cmdExit).MouseEnter += cmdExit_MouseEnter;
				((Control)_cmdExit).Click += cmdExit_Click;
			}
		}
	}

	public virtual Button cmdAddWallFloor
	{
		get
		{
			return _cmdAddWallFloor;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdAddWallFloor != null)
			{
			}
			_cmdAddWallFloor = value;
			if (_cmdAddWallFloor == null)
			{
			}
		}
	}

	public virtual Button cmdBatchAdd
	{
		get
		{
			return _cmdBatchAdd;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdBatchAdd != null)
			{
				((Control)_cmdBatchAdd).MouseLeave -= cmdBatchAdd_MouseLeave;
				((Control)_cmdBatchAdd).MouseEnter -= cmdBatchAdd_MouseEnter;
				((Control)_cmdBatchAdd).Click -= cmdBatchAdd_Click;
			}
			_cmdBatchAdd = value;
			if (_cmdBatchAdd != null)
			{
				((Control)_cmdBatchAdd).MouseLeave += cmdBatchAdd_MouseLeave;
				((Control)_cmdBatchAdd).MouseEnter += cmdBatchAdd_MouseEnter;
				((Control)_cmdBatchAdd).Click += cmdBatchAdd_Click;
			}
		}
	}

	public virtual Button Command1
	{
		get
		{
			return _Command1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Command1 != null)
			{
				((Control)_Command1).MouseLeave -= Command1_MouseLeave;
				((Control)_Command1).MouseEnter -= Command1_MouseEnter;
				((Control)_Command1).Click -= Command1_Click;
			}
			_Command1 = value;
			if (_Command1 != null)
			{
				((Control)_Command1).MouseLeave += Command1_MouseLeave;
				((Control)_Command1).MouseEnter += Command1_MouseEnter;
				((Control)_Command1).Click += Command1_Click;
			}
		}
	}

	public virtual Label Label7
	{
		get
		{
			return _Label7;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label7 != null)
			{
			}
			_Label7 = value;
			if (_Label7 == null)
			{
			}
		}
	}

	public virtual Label Label6
	{
		get
		{
			return _Label6;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label6 != null)
			{
			}
			_Label6 = value;
			if (_Label6 == null)
			{
			}
		}
	}

	public virtual Label Label5
	{
		get
		{
			return _Label5;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label5 != null)
			{
			}
			_Label5 = value;
			if (_Label5 == null)
			{
			}
		}
	}

	public virtual Button cmdCancel
	{
		get
		{
			return _cmdCancel;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdCancel != null)
			{
				((Control)_cmdCancel).MouseLeave -= cmdCancel_MouseLeave;
				((Control)_cmdCancel).MouseEnter -= cmdCancel_MouseEnter;
				((Control)_cmdCancel).Click -= cmdCancel_Click;
			}
			_cmdCancel = value;
			if (_cmdCancel != null)
			{
				((Control)_cmdCancel).MouseLeave += cmdCancel_MouseLeave;
				((Control)_cmdCancel).MouseEnter += cmdCancel_MouseEnter;
				((Control)_cmdCancel).Click += cmdCancel_Click;
			}
		}
	}

	public virtual Button cmdAddItem
	{
		get
		{
			return _cmdAddItem;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cmdAddItem != null)
			{
				((Control)_cmdAddItem).Click -= cmdAddItem_Click;
				((Control)_cmdAddItem).MouseLeave -= cmdAddItem_MouseLeave;
				((Control)_cmdAddItem).MouseEnter -= cmdAddItem_MouseEnter;
			}
			_cmdAddItem = value;
			if (_cmdAddItem != null)
			{
				((Control)_cmdAddItem).Click += cmdAddItem_Click;
				((Control)_cmdAddItem).MouseLeave += cmdAddItem_MouseLeave;
				((Control)_cmdAddItem).MouseEnter += cmdAddItem_MouseEnter;
			}
		}
	}

	public virtual TextBox txtGroup
	{
		get
		{
			return _txtGroup;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtGroup != null)
			{
			}
			_txtGroup = value;
			if (_txtGroup == null)
			{
			}
		}
	}

	public virtual TextBox txtFileName
	{
		get
		{
			return _txtFileName;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtFileName != null)
			{
			}
			_txtFileName = value;
			if (_txtFileName == null)
			{
			}
		}
	}

	public virtual TextBox txtGUID
	{
		get
		{
			return _txtGUID;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtGUID != null)
			{
			}
			_txtGUID = value;
			if (_txtGUID == null)
			{
			}
		}
	}

	public virtual TextBox txtCTSSDesc
	{
		get
		{
			return _txtCTSSDesc;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtCTSSDesc != null)
			{
			}
			_txtCTSSDesc = value;
			if (_txtCTSSDesc == null)
			{
			}
		}
	}

	public virtual TextBox txtCTSSName
	{
		get
		{
			return _txtCTSSName;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtCTSSName != null)
			{
			}
			_txtCTSSName = value;
			if (_txtCTSSName == null)
			{
			}
		}
	}

	public virtual Label Label4
	{
		get
		{
			return _Label4;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label4 != null)
			{
			}
			_Label4 = value;
			if (_Label4 == null)
			{
			}
		}
	}

	public virtual Label Label3
	{
		get
		{
			return _Label3;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label3 != null)
			{
			}
			_Label3 = value;
			if (_Label3 == null)
			{
			}
		}
	}

	public virtual Label Label2
	{
		get
		{
			return _Label2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label2 != null)
			{
			}
			_Label2 = value;
			if (_Label2 == null)
			{
			}
		}
	}

	public virtual Label Label1
	{
		get
		{
			return _Label1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label1 != null)
			{
			}
			_Label1 = value;
			if (_Label1 == null)
			{
			}
		}
	}

	public virtual ListBox lstListOfItems
	{
		get
		{
			return _lstListOfItems;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstListOfItems != null)
			{
			}
			_lstListOfItems = value;
			if (_lstListOfItems == null)
			{
			}
		}
	}

	internal virtual ListBox lstABC1
	{
		get
		{
			return _lstABC1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstABC1 != null)
			{
			}
			_lstABC1 = value;
			if (_lstABC1 == null)
			{
			}
		}
	}

	internal virtual ListBox lstRecursive
	{
		get
		{
			return _lstRecursive;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstRecursive != null)
			{
			}
			_lstRecursive = value;
			if (_lstRecursive == null)
			{
			}
		}
	}

	internal virtual SaveFileDialog cdlSaveFile
	{
		get
		{
			return _cdlSaveFile;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cdlSaveFile != null)
			{
			}
			_cdlSaveFile = value;
			if (_cdlSaveFile == null)
			{
			}
		}
	}

	internal virtual FolderBrowserDialog FolderBrowserDialog1
	{
		get
		{
			return _FolderBrowserDialog1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_FolderBrowserDialog1 != null)
			{
			}
			_FolderBrowserDialog1 = value;
			if (_FolderBrowserDialog1 == null)
			{
			}
		}
	}

	internal virtual OpenFileDialog cdlOpenFile
	{
		get
		{
			return _cdlOpenFile;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_cdlOpenFile != null)
			{
			}
			_cdlOpenFile = value;
			if (_cdlOpenFile == null)
			{
			}
		}
	}

	public virtual ToolTip ToolTip1
	{
		get
		{
			return _ToolTip1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_ToolTip1 != null)
			{
			}
			_ToolTip1 = value;
			if (_ToolTip1 == null)
			{
			}
		}
	}

	internal virtual GroupBox GroupBox3
	{
		get
		{
			return _GroupBox3;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_GroupBox3 != null)
			{
			}
			_GroupBox3 = value;
			if (_GroupBox3 == null)
			{
			}
		}
	}

	internal virtual Label lblProgress
	{
		get
		{
			return _lblProgress;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lblProgress != null)
			{
			}
			_lblProgress = value;
			if (_lblProgress == null)
			{
			}
		}
	}

	internal virtual ProgressBar ProgressBar1
	{
		get
		{
			return _ProgressBar1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_ProgressBar1 != null)
			{
			}
			_ProgressBar1 = value;
			if (_ProgressBar1 == null)
			{
			}
		}
	}

	internal virtual Label lblProgressDesc
	{
		get
		{
			return _lblProgressDesc;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lblProgressDesc != null)
			{
			}
			_lblProgressDesc = value;
			if (_lblProgressDesc == null)
			{
			}
		}
	}

	public virtual GroupBox GroupBox4
	{
		get
		{
			return _GroupBox4;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_GroupBox4 != null)
			{
			}
			_GroupBox4 = value;
			if (_GroupBox4 == null)
			{
			}
		}
	}

	internal virtual TextBox txtCollName
	{
		get
		{
			return _txtCollName;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_txtCollName != null)
			{
			}
			_txtCollName = value;
			if (_txtCollName == null)
			{
			}
		}
	}

	internal virtual PictureBox PictureBox1
	{
		get
		{
			return _PictureBox1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_PictureBox1 != null)
			{
			}
			_PictureBox1 = value;
			if (_PictureBox1 == null)
			{
			}
		}
	}

	public virtual Label Label13
	{
		get
		{
			return _Label13;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label13 != null)
			{
			}
			_Label13 = value;
			if (_Label13 == null)
			{
			}
		}
	}

	internal virtual ListBox lstCategories
	{
		get
		{
			return _lstCategories;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstCategories != null)
			{
			}
			_lstCategories = value;
			if (_lstCategories == null)
			{
			}
		}
	}

	public virtual Label Label14
	{
		get
		{
			return _Label14;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label14 != null)
			{
			}
			_Label14 = value;
			if (_Label14 == null)
			{
			}
		}
	}

	internal virtual Label lblBatchAddTotal
	{
		get
		{
			return _lblBatchAddTotal;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lblBatchAddTotal != null)
			{
			}
			_lblBatchAddTotal = value;
			if (_lblBatchAddTotal == null)
			{
			}
		}
	}

	internal virtual Label Label12
	{
		get
		{
			return _Label12;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label12 != null)
			{
			}
			_Label12 = value;
			if (_Label12 == null)
			{
			}
		}
	}

	internal virtual ListBox lstBatchCategories
	{
		get
		{
			return _lstBatchCategories;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstBatchCategories != null)
			{
				_lstBatchCategories.SelectedIndexChanged -= lstBatchCategories_SelectedIndexChanged;
			}
			_lstBatchCategories = value;
			if (_lstBatchCategories != null)
			{
				_lstBatchCategories.SelectedIndexChanged += lstBatchCategories_SelectedIndexChanged;
			}
		}
	}

	internal virtual StatusBar StatusBar1
	{
		get
		{
			return _StatusBar1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_StatusBar1 != null)
			{
			}
			_StatusBar1 = value;
			if (_StatusBar1 == null)
			{
			}
		}
	}

	internal virtual StatusBarPanel Panel1
	{
		get
		{
			return _Panel1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Panel1 != null)
			{
			}
			_Panel1 = value;
			if (_Panel1 == null)
			{
			}
		}
	}

	internal virtual CheckBox chkCompression
	{
		get
		{
			return _chkCompression;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_chkCompression != null)
			{
			}
			_chkCompression = value;
			if (_chkCompression == null)
			{
			}
		}
	}

	internal virtual ListBox lstBatchFileList
	{
		get
		{
			return _lstBatchFileList;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_lstBatchFileList != null)
			{
			}
			_lstBatchFileList = value;
			if (_lstBatchFileList == null)
			{
			}
		}
	}

	internal virtual Label Label15
	{
		get
		{
			return _Label15;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_Label15 != null)
			{
			}
			_Label15 = value;
			if (_Label15 == null)
			{
			}
		}
	}

	internal virtual PictureBox PictureBox2
	{
		get
		{
			return _PictureBox2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_PictureBox2 != null)
			{
			}
			_PictureBox2 = value;
			if (_PictureBox2 == null)
			{
			}
		}
	}

	[STAThread]
	public static void Main()
	{
		Application.Run((Form)(object)new frmMain());
	}

	public frmMain()
	{
		((Form)this).Load += frmMain_Load;
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	[DebuggerStepThrough]
	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_08de: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Expected O, but got Unknown
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Expected O, but got Unknown
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Expected O, but got Unknown
		//IL_0a68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a72: Expected O, but got Unknown
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9d: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Expected O, but got Unknown
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Expected O, but got Unknown
		//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdd: Expected O, but got Unknown
		//IL_0cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Expected O, but got Unknown
		//IL_137a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1384: Expected O, but got Unknown
		//IL_13a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13af: Expected O, but got Unknown
		//IL_1462: Unknown result type (might be due to invalid IL or missing references)
		//IL_146c: Expected O, but got Unknown
		//IL_148d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1497: Expected O, but got Unknown
		//IL_18fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1904: Expected O, but got Unknown
		//IL_19b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bf: Expected O, but got Unknown
		//IL_1a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a86: Expected O, but got Unknown
		//IL_1b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b64: Expected O, but got Unknown
		//IL_1c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3e: Expected O, but got Unknown
		//IL_1d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0a: Expected O, but got Unknown
		//IL_1dc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd1: Expected O, but got Unknown
		//IL_1df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfc: Expected O, but got Unknown
		//IL_1eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb9: Expected O, but got Unknown
		//IL_1eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee4: Expected O, but got Unknown
		//IL_1f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa1: Expected O, but got Unknown
		//IL_1fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fcc: Expected O, but got Unknown
		//IL_2073: Unknown result type (might be due to invalid IL or missing references)
		//IL_207d: Expected O, but got Unknown
		//IL_209e: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a8: Expected O, but got Unknown
		//IL_215b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2165: Expected O, but got Unknown
		//IL_2186: Unknown result type (might be due to invalid IL or missing references)
		//IL_2190: Expected O, but got Unknown
		//IL_2240: Unknown result type (might be due to invalid IL or missing references)
		//IL_224a: Expected O, but got Unknown
		//IL_226b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2275: Expected O, but got Unknown
		//IL_2330: Unknown result type (might be due to invalid IL or missing references)
		//IL_233a: Expected O, but got Unknown
		//IL_235b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2365: Expected O, but got Unknown
		//IL_241c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2426: Expected O, but got Unknown
		//IL_2447: Unknown result type (might be due to invalid IL or missing references)
		//IL_2451: Expected O, but got Unknown
		//IL_2510: Unknown result type (might be due to invalid IL or missing references)
		//IL_251a: Expected O, but got Unknown
		//IL_253b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2545: Expected O, but got Unknown
		//IL_25fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2608: Expected O, but got Unknown
		//IL_2629: Unknown result type (might be due to invalid IL or missing references)
		//IL_2633: Expected O, but got Unknown
		//IL_26f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2700: Expected O, but got Unknown
		//IL_2721: Unknown result type (might be due to invalid IL or missing references)
		//IL_272b: Expected O, but got Unknown
		//IL_27df: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e9: Expected O, but got Unknown
		//IL_289a: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a4: Expected O, but got Unknown
		//IL_2952: Unknown result type (might be due to invalid IL or missing references)
		//IL_295c: Expected O, but got Unknown
		//IL_2b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4e: Expected O, but got Unknown
		//IL_2c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c21: Expected O, but got Unknown
		//IL_2d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4d: Expected O, but got Unknown
		//IL_2e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e68: Expected O, but got Unknown
		//IL_2e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e93: Expected O, but got Unknown
		//IL_2f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f44: Expected O, but got Unknown
		//IL_2f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6f: Expected O, but got Unknown
		//IL_302e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3038: Expected O, but got Unknown
		//IL_3119: Unknown result type (might be due to invalid IL or missing references)
		//IL_3123: Expected O, but got Unknown
		//IL_3200: Unknown result type (might be due to invalid IL or missing references)
		//IL_320a: Expected O, but got Unknown
		//IL_32e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_32f1: Expected O, but got Unknown
		//IL_33e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_33ea: Expected O, but got Unknown
		//IL_34af: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b9: Expected O, but got Unknown
		//IL_356a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3574: Expected O, but got Unknown
		//IL_3621: Unknown result type (might be due to invalid IL or missing references)
		//IL_362b: Expected O, but got Unknown
		//IL_36d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e2: Expected O, but got Unknown
		//IL_378f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3799: Expected O, but got Unknown
		//IL_3be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bed: Expected O, but got Unknown
		//IL_3eec: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef6: Expected O, but got Unknown
		components = new Container();
		ResourceManager resourceManager = new ResourceManager(typeof(frmMain));
		btnDEBUG = new Button();
		GroupBox2 = new GroupBox();
		PictureBox2 = new PictureBox();
		Label15 = new Label();
		lstBatchCategories = new ListBox();
		Label12 = new Label();
		cmdCancelBatchAdd = new Button();
		cmdFinishBatchAdd = new Button();
		cmdBatchAddUp = new Button();
		cmdBatchAddDown = new Button();
		cmdBatchAddRemove = new Button();
		lblBatchAddTotal = new Label();
		lstBatchAdd = new ListBox();
		cmdAlphaSort = new Button();
		GroupBox1 = new GroupBox();
		cmdCloseOptions = new Button();
		chkWarningOff = new CheckBox();
		Label9 = new Label();
		cmdFindCollDir = new Button();
		Label8 = new Label();
		txtCollDir = new TextBox();
		cmdFindThumbDir = new Button();
		txtThumbDir = new TextBox();
		Label11 = new Label();
		chkCompression = new CheckBox();
		Label10 = new Label();
		cmdAbout = new Button();
		cmdOptions = new Button();
		txtImgPath = new TextBox();
		lstDatFileNames = new ListBox();
		txtCollID = new TextBox();
		txtPackagePath = new TextBox();
		lstInstance2 = new ListBox();
		lstInstance = new ListBox();
		lstGroups = new ListBox();
		lstResources = new ListBox();
		txtIndexSize = new TextBox();
		txtResourceCount = new TextBox();
		lstSize = new ListBox();
		lstOffset = new ListBox();
		cmbCollType = new ComboBox();
		cmdLoadPic = new Button();
		Picture1 = new PictureBox();
		txtCollName = new TextBox();
		cmdMoveDown = new Button();
		cmdRemoveItem = new Button();
		cmdMoveUp = new Button();
		cmdBackUpColl = new Button();
		cmdEditColl = new Button();
		cmdMakeNewColl = new Button();
		cmdSaveColl = new Button();
		cmdExit = new Button();
		cmdAddWallFloor = new Button();
		cmdBatchAdd = new Button();
		Command1 = new Button();
		Label7 = new Label();
		Label6 = new Label();
		Label5 = new Label();
		GroupBox4 = new GroupBox();
		Label14 = new Label();
		lstCategories = new ListBox();
		Label13 = new Label();
		PictureBox1 = new PictureBox();
		cmdCancel = new Button();
		cmdAddItem = new Button();
		txtGroup = new TextBox();
		txtFileName = new TextBox();
		txtGUID = new TextBox();
		txtCTSSDesc = new TextBox();
		txtCTSSName = new TextBox();
		Label4 = new Label();
		Label3 = new Label();
		Label2 = new Label();
		Label1 = new Label();
		lstListOfItems = new ListBox();
		lstABC1 = new ListBox();
		lstRecursive = new ListBox();
		cdlSaveFile = new SaveFileDialog();
		FolderBrowserDialog1 = new FolderBrowserDialog();
		cdlOpenFile = new OpenFileDialog();
		ToolTip1 = new ToolTip(components);
		GroupBox3 = new GroupBox();
		lblProgress = new Label();
		ProgressBar1 = new ProgressBar();
		lblProgressDesc = new Label();
		StatusBar1 = new StatusBar();
		Panel1 = new StatusBarPanel();
		lstBatchFileList = new ListBox();
		((Control)GroupBox2).SuspendLayout();
		((Control)GroupBox1).SuspendLayout();
		((Control)GroupBox4).SuspendLayout();
		((Control)GroupBox3).SuspendLayout();
		((ISupportInitialize)Panel1).BeginInit();
		((Control)this).SuspendLayout();
		Button obj = btnDEBUG;
		Point location = new Point(192, 463);
		((Control)obj).Location = location;
		((Control)btnDEBUG).Name = "btnDEBUG";
		Button obj2 = btnDEBUG;
		Size size = new Size(32, 32);
		((Control)obj2).Size = size;
		((Control)btnDEBUG).TabIndex = 99;
		((Control)btnDEBUG).Text = "Button1";
		((Control)btnDEBUG).Visible = false;
		((Control)GroupBox2).Controls.Add((Control)(object)PictureBox2);
		((Control)GroupBox2).Controls.Add((Control)(object)Label15);
		((Control)GroupBox2).Controls.Add((Control)(object)lstBatchCategories);
		((Control)GroupBox2).Controls.Add((Control)(object)Label12);
		((Control)GroupBox2).Controls.Add((Control)(object)cmdCancelBatchAdd);
		((Control)GroupBox2).Controls.Add((Control)(object)cmdFinishBatchAdd);
		((Control)GroupBox2).Controls.Add((Control)(object)cmdBatchAddUp);
		((Control)GroupBox2).Controls.Add((Control)(object)cmdBatchAddDown);
		((Control)GroupBox2).Controls.Add((Control)(object)cmdBatchAddRemove);
		((Control)GroupBox2).Controls.Add((Control)(object)lblBatchAddTotal);
		((Control)GroupBox2).Controls.Add((Control)(object)lstBatchAdd);
		GroupBox groupBox = GroupBox2;
		location = new Point(16, 80);
		((Control)groupBox).Location = location;
		((Control)GroupBox2).Name = "GroupBox2";
		GroupBox groupBox2 = GroupBox2;
		size = new Size(608, 376);
		((Control)groupBox2).Size = size;
		((Control)GroupBox2).TabIndex = 95;
		GroupBox2.TabStop = false;
		GroupBox2.Text = "Batch Adding Pane";
		((Control)GroupBox2).Visible = false;
		PictureBox pictureBox = PictureBox2;
		location = new Point(472, 240);
		((Control)pictureBox).Location = location;
		((Control)PictureBox2).Name = "PictureBox2";
		PictureBox pictureBox2 = PictureBox2;
		size = new Size(128, 128);
		((Control)pictureBox2).Size = size;
		PictureBox2.TabIndex = 33;
		PictureBox2.TabStop = false;
		Label label = Label15;
		location = new Point(472, 224);
		((Control)label).Location = location;
		((Control)Label15).Name = "Label15";
		Label label2 = Label15;
		size = new Size(128, 16);
		((Control)label2).Size = size;
		((Control)Label15).TabIndex = 32;
		((Control)Label15).Text = "Object Preview:";
		Label15.TextAlign = (ContentAlignment)2;
		ListBox obj3 = lstBatchCategories;
		location = new Point(472, 128);
		((Control)obj3).Location = location;
		((Control)lstBatchCategories).Name = "lstBatchCategories";
		ListBox obj4 = lstBatchCategories;
		size = new Size(128, 95);
		((Control)obj4).Size = size;
		lstBatchCategories.Sorted = true;
		((Control)lstBatchCategories).TabIndex = 31;
		Label label3 = Label12;
		location = new Point(472, 112);
		((Control)label3).Location = location;
		((Control)Label12).Name = "Label12";
		Label label4 = Label12;
		size = new Size(128, 16);
		((Control)label4).Size = size;
		((Control)Label12).TabIndex = 30;
		((Control)Label12).Text = "Categories of Objects:";
		Label12.TextAlign = (ContentAlignment)2;
		((Control)cmdCancelBatchAdd).BackColor = SystemColors.Control;
		((Control)cmdCancelBatchAdd).Cursor = Cursors.Default;
		((Control)cmdCancelBatchAdd).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdCancelBatchAdd).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdCancelBatchAdd).Image = (Image)resourceManager.GetObject("cmdCancelBatchAdd.Image");
		Button obj5 = cmdCancelBatchAdd;
		location = new Point(280, 328);
		((Control)obj5).Location = location;
		((Control)cmdCancelBatchAdd).Name = "cmdCancelBatchAdd";
		((Control)cmdCancelBatchAdd).RightToLeft = (RightToLeft)0;
		Button obj6 = cmdCancelBatchAdd;
		size = new Size(44, 44);
		((Control)obj6).Size = size;
		((Control)cmdCancelBatchAdd).TabIndex = 29;
		ToolTip1.SetToolTip((Control)(object)cmdCancelBatchAdd, "Cancel");
		((Control)cmdFinishBatchAdd).BackColor = SystemColors.Control;
		((Control)cmdFinishBatchAdd).Cursor = Cursors.Default;
		((Control)cmdFinishBatchAdd).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdFinishBatchAdd).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdFinishBatchAdd).Image = (Image)resourceManager.GetObject("cmdFinishBatchAdd.Image");
		Button obj7 = cmdFinishBatchAdd;
		location = new Point(224, 328);
		((Control)obj7).Location = location;
		((Control)cmdFinishBatchAdd).Name = "cmdFinishBatchAdd";
		((Control)cmdFinishBatchAdd).RightToLeft = (RightToLeft)0;
		Button obj8 = cmdFinishBatchAdd;
		size = new Size(44, 44);
		((Control)obj8).Size = size;
		((Control)cmdFinishBatchAdd).TabIndex = 28;
		ToolTip1.SetToolTip((Control)(object)cmdFinishBatchAdd, "OK");
		((Control)cmdBatchAddUp).BackColor = SystemColors.Control;
		((Control)cmdBatchAddUp).Cursor = Cursors.Default;
		((Control)cmdBatchAddUp).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdBatchAddUp).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdBatchAddUp).Image = (Image)resourceManager.GetObject("cmdBatchAddUp.Image");
		Button obj9 = cmdBatchAddUp;
		location = new Point(472, 16);
		((Control)obj9).Location = location;
		((Control)cmdBatchAddUp).Name = "cmdBatchAddUp";
		((Control)cmdBatchAddUp).RightToLeft = (RightToLeft)0;
		Button obj10 = cmdBatchAddUp;
		size = new Size(44, 44);
		((Control)obj10).Size = size;
		((Control)cmdBatchAddUp).TabIndex = 27;
		ToolTip1.SetToolTip((Control)(object)cmdBatchAddUp, "Move Item Up in List");
		((Control)cmdBatchAddDown).BackColor = SystemColors.Control;
		((Control)cmdBatchAddDown).Cursor = Cursors.Default;
		((Control)cmdBatchAddDown).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdBatchAddDown).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdBatchAddDown).Image = (Image)resourceManager.GetObject("cmdBatchAddDown.Image");
		Button obj11 = cmdBatchAddDown;
		location = new Point(472, 64);
		((Control)obj11).Location = location;
		((Control)cmdBatchAddDown).Name = "cmdBatchAddDown";
		((Control)cmdBatchAddDown).RightToLeft = (RightToLeft)0;
		Button obj12 = cmdBatchAddDown;
		size = new Size(44, 44);
		((Control)obj12).Size = size;
		((Control)cmdBatchAddDown).TabIndex = 26;
		ToolTip1.SetToolTip((Control)(object)cmdBatchAddDown, "Move Item Down in List");
		((Control)cmdBatchAddRemove).BackColor = SystemColors.Control;
		((Control)cmdBatchAddRemove).Cursor = Cursors.Default;
		((Control)cmdBatchAddRemove).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdBatchAddRemove).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdBatchAddRemove).Image = (Image)resourceManager.GetObject("cmdBatchAddRemove.Image");
		Button obj13 = cmdBatchAddRemove;
		location = new Point(520, 40);
		((Control)obj13).Location = location;
		((Control)cmdBatchAddRemove).Name = "cmdBatchAddRemove";
		((Control)cmdBatchAddRemove).RightToLeft = (RightToLeft)0;
		Button obj14 = cmdBatchAddRemove;
		size = new Size(44, 44);
		((Control)obj14).Size = size;
		((Control)cmdBatchAddRemove).TabIndex = 25;
		ToolTip1.SetToolTip((Control)(object)cmdBatchAddRemove, "Remove Item From List");
		Label obj15 = lblBatchAddTotal;
		location = new Point(8, 16);
		((Control)obj15).Location = location;
		((Control)lblBatchAddTotal).Name = "lblBatchAddTotal";
		Label obj16 = lblBatchAddTotal;
		size = new Size(456, 16);
		((Control)obj16).Size = size;
		((Control)lblBatchAddTotal).TabIndex = 2;
		((Control)lblBatchAddTotal).Text = "Total Items: 0";
		lblBatchAddTotal.TextAlign = (ContentAlignment)2;
		ListBox obj17 = lstBatchAdd;
		location = new Point(8, 32);
		((Control)obj17).Location = location;
		((Control)lstBatchAdd).Name = "lstBatchAdd";
		ListBox obj18 = lstBatchAdd;
		size = new Size(448, 290);
		((Control)obj18).Size = size;
		((Control)lstBatchAdd).TabIndex = 0;
		((Control)cmdAlphaSort).BackColor = SystemColors.Control;
		((Control)cmdAlphaSort).Cursor = Cursors.Default;
		((Control)cmdAlphaSort).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdAlphaSort).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdAlphaSort).Image = (Image)resourceManager.GetObject("cmdAlphaSort.Image");
		Button obj19 = cmdAlphaSort;
		location = new Point(344, 7);
		((Control)obj19).Location = location;
		((Control)cmdAlphaSort).Name = "cmdAlphaSort";
		((Control)cmdAlphaSort).RightToLeft = (RightToLeft)0;
		Button obj20 = cmdAlphaSort;
		size = new Size(67, 67);
		((Control)obj20).Size = size;
		((Control)cmdAlphaSort).TabIndex = 97;
		ToolTip1.SetToolTip((Control)(object)cmdAlphaSort, "Sort Collections into Alphabetical Order");
		((Control)GroupBox1).Controls.Add((Control)(object)cmdCloseOptions);
		((Control)GroupBox1).Controls.Add((Control)(object)chkWarningOff);
		((Control)GroupBox1).Controls.Add((Control)(object)Label9);
		((Control)GroupBox1).Controls.Add((Control)(object)cmdFindCollDir);
		((Control)GroupBox1).Controls.Add((Control)(object)Label8);
		((Control)GroupBox1).Controls.Add((Control)(object)txtCollDir);
		((Control)GroupBox1).Controls.Add((Control)(object)cmdFindThumbDir);
		((Control)GroupBox1).Controls.Add((Control)(object)txtThumbDir);
		((Control)GroupBox1).Controls.Add((Control)(object)Label11);
		((Control)GroupBox1).Controls.Add((Control)(object)chkCompression);
		((Control)GroupBox1).Controls.Add((Control)(object)Label10);
		GroupBox groupBox3 = GroupBox1;
		location = new Point(88, 96);
		((Control)groupBox3).Location = location;
		((Control)GroupBox1).Name = "GroupBox1";
		GroupBox groupBox4 = GroupBox1;
		size = new Size(456, 304);
		((Control)groupBox4).Size = size;
		((Control)GroupBox1).TabIndex = 94;
		GroupBox1.TabStop = false;
		GroupBox1.Text = "Options Menu";
		((Control)GroupBox1).Visible = false;
		Button obj21 = cmdCloseOptions;
		location = new Point(184, 272);
		((Control)obj21).Location = location;
		((Control)cmdCloseOptions).Name = "cmdCloseOptions";
		Button obj22 = cmdCloseOptions;
		size = new Size(104, 24);
		((Control)obj22).Size = size;
		((Control)cmdCloseOptions).TabIndex = 10;
		((Control)cmdCloseOptions).Text = "OK";
		ToolTip1.SetToolTip((Control)(object)cmdCloseOptions, "OK");
		CheckBox obj23 = chkWarningOff;
		location = new Point(224, 88);
		((Control)obj23).Location = location;
		((Control)chkWarningOff).Name = "chkWarningOff";
		CheckBox obj24 = chkWarningOff;
		size = new Size(104, 16);
		((Control)obj24).Size = size;
		((Control)chkWarningOff).TabIndex = 4;
		((Control)chkWarningOff).Text = "Yes";
		Label label5 = Label9;
		location = new Point(16, 80);
		((Control)label5).Location = location;
		((Control)Label9).Name = "Label9";
		Label label6 = Label9;
		size = new Size(200, 32);
		((Control)label6).Size = size;
		((Control)Label9).TabIndex = 3;
		((Control)Label9).Text = "Turn Off Warning Dialogs - Warning dialogs will not show.";
		Button obj25 = cmdFindCollDir;
		location = new Point(424, 24);
		((Control)obj25).Location = location;
		((Control)cmdFindCollDir).Name = "cmdFindCollDir";
		Button obj26 = cmdFindCollDir;
		size = new Size(24, 24);
		((Control)obj26).Size = size;
		((Control)cmdFindCollDir).TabIndex = 2;
		((Control)cmdFindCollDir).Text = "...";
		ToolTip1.SetToolTip((Control)(object)cmdFindCollDir, "Browse for Folder");
		Label label7 = Label8;
		location = new Point(16, 24);
		((Control)label7).Location = location;
		((Control)Label8).Name = "Label8";
		Label label8 = Label8;
		size = new Size(200, 48);
		((Control)label8).Size = size;
		((Control)Label8).TabIndex = 1;
		((Control)Label8).Text = "Default Collections Directory - This is where the program will go automatically when you choose to save or edit a collection file.";
		TextBox obj27 = txtCollDir;
		location = new Point(224, 24);
		((Control)obj27).Location = location;
		((Control)txtCollDir).Name = "txtCollDir";
		TextBox obj28 = txtCollDir;
		size = new Size(192, 20);
		((Control)obj28).Size = size;
		((Control)txtCollDir).TabIndex = 0;
		txtCollDir.Text = "";
		Button obj29 = cmdFindThumbDir;
		location = new Point(424, 208);
		((Control)obj29).Location = location;
		((Control)cmdFindThumbDir).Name = "cmdFindThumbDir";
		Button obj30 = cmdFindThumbDir;
		size = new Size(24, 24);
		((Control)obj30).Size = size;
		((Control)cmdFindThumbDir).TabIndex = 9;
		((Control)cmdFindThumbDir).Text = "...";
		ToolTip1.SetToolTip((Control)(object)cmdFindThumbDir, "Browse for Folder");
		TextBox obj31 = txtThumbDir;
		location = new Point(224, 208);
		((Control)obj31).Location = location;
		((Control)txtThumbDir).Name = "txtThumbDir";
		TextBox obj32 = txtThumbDir;
		size = new Size(192, 20);
		((Control)obj32).Size = size;
		((Control)txtThumbDir).TabIndex = 8;
		txtThumbDir.Text = "";
		Label label9 = Label11;
		location = new Point(16, 192);
		((Control)label9).Location = location;
		((Control)Label11).Name = "Label11";
		Label label10 = Label11;
		size = new Size(200, 64);
		((Control)label10).Size = size;
		((Control)Label11).TabIndex = 7;
		((Control)Label11).Text = "Thumbnail Packages Directory - This is the directory that your thumbnail pacakges are located in. Normally located in your My Documents/EA Games/The Sims 2/Thumbnails folder.";
		CheckBox obj33 = chkCompression;
		location = new Point(224, 144);
		((Control)obj33).Location = location;
		((Control)chkCompression).Name = "chkCompression";
		CheckBox obj34 = chkCompression;
		size = new Size(104, 16);
		((Control)obj34).Size = size;
		((Control)chkCompression).TabIndex = 6;
		((Control)chkCompression).Text = "Yes";
		Label label11 = Label10;
		location = new Point(16, 120);
		((Control)label11).Location = location;
		((Control)Label10).Name = "Label10";
		Label label12 = Label10;
		size = new Size(200, 64);
		((Control)label12).Size = size;
		((Control)Label10).TabIndex = 5;
		((Control)Label10).Text = "Enable Package Compression? (Checked means yes, unchecked means no. MAC USERS: You MUST leave this unchecked or else saving files will not function.";
		((Control)cmdAbout).BackColor = SystemColors.Control;
		((Control)cmdAbout).Cursor = Cursors.Default;
		((Control)cmdAbout).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdAbout).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdAbout).Image = (Image)resourceManager.GetObject("cmdAbout.Image");
		Button obj35 = cmdAbout;
		location = new Point(560, 7);
		((Control)obj35).Location = location;
		((Control)cmdAbout).Name = "cmdAbout";
		((Control)cmdAbout).RightToLeft = (RightToLeft)0;
		Button obj36 = cmdAbout;
		size = new Size(67, 67);
		((Control)obj36).Size = size;
		((Control)cmdAbout).TabIndex = 93;
		((ButtonBase)cmdAbout).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)cmdAbout, "About This Program");
		((Control)cmdOptions).BackColor = SystemColors.Control;
		((Control)cmdOptions).Cursor = Cursors.Default;
		((Control)cmdOptions).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdOptions).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdOptions).Image = (Image)resourceManager.GetObject("cmdOptions.Image");
		Button obj37 = cmdOptions;
		location = new Point(456, 7);
		((Control)obj37).Location = location;
		((Control)cmdOptions).Name = "cmdOptions";
		((Control)cmdOptions).RightToLeft = (RightToLeft)0;
		Button obj38 = cmdOptions;
		size = new Size(67, 67);
		((Control)obj38).Size = size;
		((Control)cmdOptions).TabIndex = 92;
		((ButtonBase)cmdOptions).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)cmdOptions, "Options");
		TextBox obj39 = txtImgPath;
		location = new Point(328, 111);
		((Control)obj39).Location = location;
		((Control)txtImgPath).Name = "txtImgPath";
		TextBox obj40 = txtImgPath;
		size = new Size(208, 20);
		((Control)obj40).Size = size;
		((Control)txtImgPath).TabIndex = 91;
		txtImgPath.Text = "";
		ListBox obj41 = lstDatFileNames;
		location = new Point(240, 463);
		((Control)obj41).Location = location;
		((Control)lstDatFileNames).Name = "lstDatFileNames";
		ListBox obj42 = lstDatFileNames;
		size = new Size(192, 56);
		((Control)obj42).Size = size;
		((Control)lstDatFileNames).TabIndex = 90;
		((Control)lstDatFileNames).Visible = false;
		((Control)txtCollID).Enabled = false;
		TextBox obj43 = txtCollID;
		location = new Point(416, 143);
		((Control)obj43).Location = location;
		((Control)txtCollID).Name = "txtCollID";
		TextBox obj44 = txtCollID;
		size = new Size(120, 20);
		((Control)obj44).Size = size;
		((Control)txtCollID).TabIndex = 89;
		txtCollID.Text = "";
		TextBox obj45 = txtPackagePath;
		location = new Point(80, 503);
		((Control)obj45).Location = location;
		((Control)txtPackagePath).Name = "txtPackagePath";
		TextBox obj46 = txtPackagePath;
		size = new Size(144, 20);
		((Control)obj46).Size = size;
		((Control)txtPackagePath).TabIndex = 88;
		txtPackagePath.Text = "";
		((Control)txtPackagePath).Visible = false;
		ListBox obj47 = lstInstance2;
		location = new Point(648, 455);
		((Control)obj47).Location = location;
		((Control)lstInstance2).Name = "lstInstance2";
		ListBox obj48 = lstInstance2;
		size = new Size(128, 69);
		((Control)obj48).Size = size;
		((Control)lstInstance2).TabIndex = 87;
		ListBox obj49 = lstInstance;
		location = new Point(648, 375);
		((Control)obj49).Location = location;
		((Control)lstInstance).Name = "lstInstance";
		ListBox obj50 = lstInstance;
		size = new Size(128, 69);
		((Control)obj50).Size = size;
		((Control)lstInstance).TabIndex = 86;
		ListBox obj51 = lstGroups;
		location = new Point(648, 7);
		((Control)obj51).Location = location;
		((Control)lstGroups).Name = "lstGroups";
		ListBox obj52 = lstGroups;
		size = new Size(128, 69);
		((Control)obj52).Size = size;
		((Control)lstGroups).TabIndex = 85;
		ListBox obj53 = lstResources;
		location = new Point(648, 87);
		((Control)obj53).Location = location;
		((Control)lstResources).Name = "lstResources";
		ListBox obj54 = lstResources;
		size = new Size(128, 69);
		((Control)obj54).Size = size;
		((Control)lstResources).TabIndex = 84;
		TextBox obj55 = txtIndexSize;
		location = new Point(648, 191);
		((Control)obj55).Location = location;
		((Control)txtIndexSize).Name = "txtIndexSize";
		TextBox obj56 = txtIndexSize;
		size = new Size(72, 20);
		((Control)obj56).Size = size;
		((Control)txtIndexSize).TabIndex = 83;
		txtIndexSize.Text = "";
		TextBox obj57 = txtResourceCount;
		location = new Point(648, 167);
		((Control)obj57).Location = location;
		((Control)txtResourceCount).Name = "txtResourceCount";
		TextBox obj58 = txtResourceCount;
		size = new Size(72, 20);
		((Control)obj58).Size = size;
		((Control)txtResourceCount).TabIndex = 82;
		txtResourceCount.Text = "";
		lstSize.BackColor = SystemColors.Window;
		((Control)lstSize).Cursor = Cursors.Default;
		((Control)lstSize).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		lstSize.ForeColor = SystemColors.WindowText;
		lstSize.ItemHeight = 14;
		ListBox obj59 = lstSize;
		location = new Point(648, 215);
		((Control)obj59).Location = location;
		((Control)lstSize).Name = "lstSize";
		lstSize.RightToLeft = (RightToLeft)0;
		ListBox obj60 = lstSize;
		size = new Size(128, 74);
		((Control)obj60).Size = size;
		((Control)lstSize).TabIndex = 81;
		lstOffset.BackColor = SystemColors.Window;
		((Control)lstOffset).Cursor = Cursors.Default;
		((Control)lstOffset).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		lstOffset.ForeColor = SystemColors.WindowText;
		lstOffset.ItemHeight = 14;
		ListBox obj61 = lstOffset;
		location = new Point(648, 295);
		((Control)obj61).Location = location;
		((Control)lstOffset).Name = "lstOffset";
		lstOffset.RightToLeft = (RightToLeft)0;
		ListBox obj62 = lstOffset;
		size = new Size(128, 74);
		((Control)obj62).Size = size;
		((Control)lstOffset).TabIndex = 80;
		cmbCollType.BackColor = SystemColors.Window;
		((Control)cmbCollType).Cursor = Cursors.Default;
		cmbCollType.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbCollType).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		cmbCollType.ForeColor = SystemColors.WindowText;
		cmbCollType.Items.AddRange(new object[3] { "Residential", "Community", "Both (PETS ONLY)" });
		ComboBox obj63 = cmbCollType;
		location = new Point(248, 143);
		((Control)obj63).Location = location;
		((Control)cmbCollType).Name = "cmbCollType";
		((Control)cmbCollType).RightToLeft = (RightToLeft)0;
		ComboBox obj64 = cmbCollType;
		size = new Size(153, 22);
		((Control)obj64).Size = size;
		((Control)cmbCollType).TabIndex = 79;
		((Control)cmdLoadPic).BackColor = SystemColors.Control;
		((Control)cmdLoadPic).Cursor = Cursors.Default;
		((Control)cmdLoadPic).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdLoadPic).ForeColor = SystemColors.ControlText;
		Button obj65 = cmdLoadPic;
		location = new Point(288, 103);
		((Control)obj65).Location = location;
		((Control)cmdLoadPic).Name = "cmdLoadPic";
		((Control)cmdLoadPic).RightToLeft = (RightToLeft)0;
		Button obj66 = cmdLoadPic;
		size = new Size(25, 25);
		((Control)obj66).Size = size;
		((Control)cmdLoadPic).TabIndex = 78;
		((Control)cmdLoadPic).Text = "...";
		ToolTip1.SetToolTip((Control)(object)cmdLoadPic, "Browse for Image");
		((Control)Picture1).BackColor = SystemColors.Control;
		Picture1.BorderStyle = (BorderStyle)2;
		((Control)Picture1).Cursor = Cursors.Default;
		Picture1.Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		Picture1.ForeColor = SystemColors.ControlText;
		PictureBox picture = Picture1;
		location = new Point(248, 103);
		((Control)picture).Location = location;
		((Control)Picture1).Name = "Picture1";
		Picture1.RightToLeft = (RightToLeft)0;
		PictureBox picture2 = Picture1;
		size = new Size(33, 32);
		((Control)picture2).Size = size;
		Picture1.TabIndex = 77;
		Picture1.TabStop = false;
		txtCollName.AcceptsReturn = true;
		((TextBoxBase)txtCollName).AutoSize = false;
		((TextBoxBase)txtCollName).BackColor = SystemColors.Window;
		((Control)txtCollName).Cursor = Cursors.IBeam;
		((Control)txtCollName).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((TextBoxBase)txtCollName).ForeColor = SystemColors.WindowText;
		TextBox obj67 = txtCollName;
		location = new Point(248, 80);
		((Control)obj67).Location = location;
		((Control)txtCollName).Name = "txtCollName";
		((Control)txtCollName).RightToLeft = (RightToLeft)0;
		TextBox obj68 = txtCollName;
		size = new Size(289, 19);
		((Control)obj68).Size = size;
		((Control)txtCollName).TabIndex = 76;
		txtCollName.Text = "";
		((Control)cmdMoveDown).BackColor = SystemColors.Control;
		((Control)cmdMoveDown).Cursor = Cursors.Default;
		((Control)cmdMoveDown).Enabled = false;
		((Control)cmdMoveDown).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdMoveDown).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdMoveDown).Image = (Image)resourceManager.GetObject("cmdMoveDown.Image");
		Button obj69 = cmdMoveDown;
		location = new Point(544, 311);
		((Control)obj69).Location = location;
		((Control)cmdMoveDown).Name = "cmdMoveDown";
		((Control)cmdMoveDown).RightToLeft = (RightToLeft)0;
		Button obj70 = cmdMoveDown;
		size = new Size(44, 44);
		((Control)obj70).Size = size;
		((Control)cmdMoveDown).TabIndex = 72;
		ToolTip1.SetToolTip((Control)(object)cmdMoveDown, "Move Item Down in List");
		((Control)cmdRemoveItem).BackColor = SystemColors.Control;
		((Control)cmdRemoveItem).Cursor = Cursors.Default;
		((Control)cmdRemoveItem).Enabled = false;
		((Control)cmdRemoveItem).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdRemoveItem).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdRemoveItem).Image = (Image)resourceManager.GetObject("cmdRemoveItem.Image");
		Button obj71 = cmdRemoveItem;
		location = new Point(544, 255);
		((Control)obj71).Location = location;
		((Control)cmdRemoveItem).Name = "cmdRemoveItem";
		((Control)cmdRemoveItem).RightToLeft = (RightToLeft)0;
		Button obj72 = cmdRemoveItem;
		size = new Size(44, 44);
		((Control)obj72).Size = size;
		((Control)cmdRemoveItem).TabIndex = 71;
		ToolTip1.SetToolTip((Control)(object)cmdRemoveItem, "Remove Item from List");
		((Control)cmdMoveUp).BackColor = SystemColors.Control;
		((Control)cmdMoveUp).Cursor = Cursors.Default;
		((Control)cmdMoveUp).Enabled = false;
		((Control)cmdMoveUp).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdMoveUp).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdMoveUp).Image = (Image)resourceManager.GetObject("cmdMoveUp.Image");
		Button obj73 = cmdMoveUp;
		location = new Point(544, 199);
		((Control)obj73).Location = location;
		((Control)cmdMoveUp).Name = "cmdMoveUp";
		((Control)cmdMoveUp).RightToLeft = (RightToLeft)0;
		Button obj74 = cmdMoveUp;
		size = new Size(44, 44);
		((Control)obj74).Size = size;
		((Control)cmdMoveUp).TabIndex = 70;
		ToolTip1.SetToolTip((Control)(object)cmdMoveUp, "Move Item Up in List");
		((Control)cmdBackUpColl).BackColor = SystemColors.Control;
		((Control)cmdBackUpColl).Cursor = Cursors.Default;
		((Control)cmdBackUpColl).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdBackUpColl).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdBackUpColl).Image = (Image)resourceManager.GetObject("cmdBackUpColl.Image");
		Button obj75 = cmdBackUpColl;
		location = new Point(232, 7);
		((Control)obj75).Location = location;
		((Control)cmdBackUpColl).Name = "cmdBackUpColl";
		((Control)cmdBackUpColl).RightToLeft = (RightToLeft)0;
		Button obj76 = cmdBackUpColl;
		size = new Size(67, 67);
		((Control)obj76).Size = size;
		((Control)cmdBackUpColl).TabIndex = 69;
		((ButtonBase)cmdBackUpColl).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)cmdBackUpColl, "Make Backup of Your Collections");
		((Control)cmdEditColl).BackColor = SystemColors.Control;
		((Control)cmdEditColl).Cursor = Cursors.Default;
		((Control)cmdEditColl).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdEditColl).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdEditColl).Image = (Image)resourceManager.GetObject("cmdEditColl.Image");
		Button obj77 = cmdEditColl;
		location = new Point(120, 7);
		((Control)obj77).Location = location;
		((Control)cmdEditColl).Name = "cmdEditColl";
		((Control)cmdEditColl).RightToLeft = (RightToLeft)0;
		Button obj78 = cmdEditColl;
		size = new Size(67, 67);
		((Control)obj78).Size = size;
		((Control)cmdEditColl).TabIndex = 67;
		((ButtonBase)cmdEditColl).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)cmdEditColl, "Edit a Collection");
		((Control)cmdMakeNewColl).BackColor = SystemColors.Control;
		((Control)cmdMakeNewColl).Cursor = Cursors.Default;
		((Control)cmdMakeNewColl).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdMakeNewColl).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdMakeNewColl).Image = (Image)resourceManager.GetObject("cmdMakeNewColl.Image");
		Button obj79 = cmdMakeNewColl;
		location = new Point(8, 7);
		((Control)obj79).Location = location;
		((Control)cmdMakeNewColl).Name = "cmdMakeNewColl";
		((Control)cmdMakeNewColl).RightToLeft = (RightToLeft)0;
		Button obj80 = cmdMakeNewColl;
		size = new Size(67, 67);
		((Control)obj80).Size = size;
		((Control)cmdMakeNewColl).TabIndex = 66;
		((ButtonBase)cmdMakeNewColl).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)cmdMakeNewColl, "Make New Collection");
		((Control)cmdSaveColl).BackColor = SystemColors.Control;
		((Control)cmdSaveColl).Cursor = Cursors.Default;
		((Control)cmdSaveColl).Enabled = false;
		((Control)cmdSaveColl).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSaveColl).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdSaveColl).Image = (Image)resourceManager.GetObject("cmdSaveColl.Image");
		Button obj81 = cmdSaveColl;
		location = new Point(560, 455);
		((Control)obj81).Location = location;
		((Control)cmdSaveColl).Name = "cmdSaveColl";
		((Control)cmdSaveColl).RightToLeft = (RightToLeft)0;
		Button obj82 = cmdSaveColl;
		size = new Size(67, 67);
		((Control)obj82).Size = size;
		((Control)cmdSaveColl).TabIndex = 65;
		((ButtonBase)cmdSaveColl).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)cmdSaveColl, "Save This Collection File");
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdExit).Image = (Image)resourceManager.GetObject("cmdExit.Image");
		Button obj83 = cmdExit;
		location = new Point(8, 455);
		((Control)obj83).Location = location;
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		Button obj84 = cmdExit;
		size = new Size(67, 67);
		((Control)obj84).Size = size;
		((Control)cmdExit).TabIndex = 64;
		((ButtonBase)cmdExit).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)cmdExit, "Exit");
		((Control)cmdAddWallFloor).BackColor = SystemColors.Control;
		((Control)cmdAddWallFloor).Cursor = Cursors.Default;
		((Control)cmdAddWallFloor).Enabled = false;
		((Control)cmdAddWallFloor).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdAddWallFloor).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdAddWallFloor).Image = (Image)resourceManager.GetObject("cmdAddWallFloor.Image");
		Button obj85 = cmdAddWallFloor;
		location = new Point(288, 383);
		((Control)obj85).Location = location;
		((Control)cmdAddWallFloor).Name = "cmdAddWallFloor";
		((Control)cmdAddWallFloor).RightToLeft = (RightToLeft)0;
		Button obj86 = cmdAddWallFloor;
		size = new Size(67, 67);
		((Control)obj86).Size = size;
		((Control)cmdAddWallFloor).TabIndex = 63;
		((ButtonBase)cmdAddWallFloor).TextAlign = (ContentAlignment)512;
		((Control)cmdAddWallFloor).Visible = false;
		((Control)cmdBatchAdd).BackColor = SystemColors.Control;
		((Control)cmdBatchAdd).Cursor = Cursors.Default;
		((Control)cmdBatchAdd).Enabled = false;
		((Control)cmdBatchAdd).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdBatchAdd).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdBatchAdd).Image = (Image)resourceManager.GetObject("cmdBatchAdd.Image");
		Button obj87 = cmdBatchAdd;
		location = new Point(464, 383);
		((Control)obj87).Location = location;
		((Control)cmdBatchAdd).Name = "cmdBatchAdd";
		((Control)cmdBatchAdd).RightToLeft = (RightToLeft)0;
		Button obj88 = cmdBatchAdd;
		size = new Size(67, 67);
		((Control)obj88).Size = size;
		((Control)cmdBatchAdd).TabIndex = 62;
		((ButtonBase)cmdBatchAdd).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)cmdBatchAdd, "Batch Add all Objects in a Directory to Collection");
		((Control)Command1).BackColor = SystemColors.Control;
		((Control)Command1).Cursor = Cursors.Default;
		((Control)Command1).Enabled = false;
		((Control)Command1).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Command1).ForeColor = SystemColors.ControlText;
		((ButtonBase)Command1).Image = (Image)resourceManager.GetObject("Command1.Image");
		Button command = Command1;
		location = new Point(104, 383);
		((Control)command).Location = location;
		((Control)Command1).Name = "Command1";
		((Control)Command1).RightToLeft = (RightToLeft)0;
		Button command2 = Command1;
		size = new Size(67, 67);
		((Control)command2).Size = size;
		((Control)Command1).TabIndex = 60;
		((ButtonBase)Command1).TextAlign = (ContentAlignment)512;
		ToolTip1.SetToolTip((Control)(object)Command1, "Add Object to Collection");
		((Control)Label7).BackColor = Color.Transparent;
		((Control)Label7).Cursor = Cursors.Default;
		((Control)Label7).Font = new Font("Comic Sans MS", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label7).ForeColor = SystemColors.ControlText;
		Label label13 = Label7;
		location = new Point(88, 135);
		((Control)label13).Location = location;
		((Control)Label7).Name = "Label7";
		((Control)Label7).RightToLeft = (RightToLeft)0;
		Label label14 = Label7;
		size = new Size(153, 25);
		((Control)label14).Size = size;
		((Control)Label7).TabIndex = 75;
		((Control)Label7).Text = "Collection Type:";
		((Control)Label6).BackColor = Color.Transparent;
		((Control)Label6).Cursor = Cursors.Default;
		((Control)Label6).Font = new Font("Comic Sans MS", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label6).ForeColor = SystemColors.ControlText;
		Label label15 = Label6;
		location = new Point(88, 103);
		((Control)label15).Location = location;
		((Control)Label6).Name = "Label6";
		((Control)Label6).RightToLeft = (RightToLeft)0;
		Label label16 = Label6;
		size = new Size(145, 25);
		((Control)label16).Size = size;
		((Control)Label6).TabIndex = 74;
		((Control)Label6).Text = "Picture:";
		((Control)Label5).BackColor = Color.Transparent;
		((Control)Label5).Cursor = Cursors.Default;
		((Control)Label5).Font = new Font("Comic Sans MS", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label5).ForeColor = SystemColors.ControlText;
		Label label17 = Label5;
		location = new Point(88, 79);
		((Control)label17).Location = location;
		((Control)Label5).Name = "Label5";
		((Control)Label5).RightToLeft = (RightToLeft)0;
		Label label18 = Label5;
		size = new Size(160, 25);
		((Control)label18).Size = size;
		((Control)Label5).TabIndex = 73;
		((Control)Label5).Text = "Name Of Collection:";
		((Control)GroupBox4).BackColor = SystemColors.InactiveCaptionText;
		((Control)GroupBox4).Controls.Add((Control)(object)Label14);
		((Control)GroupBox4).Controls.Add((Control)(object)lstCategories);
		((Control)GroupBox4).Controls.Add((Control)(object)Label13);
		((Control)GroupBox4).Controls.Add((Control)(object)PictureBox1);
		((Control)GroupBox4).Controls.Add((Control)(object)cmdCancel);
		((Control)GroupBox4).Controls.Add((Control)(object)cmdAddItem);
		((Control)GroupBox4).Controls.Add((Control)(object)txtGroup);
		((Control)GroupBox4).Controls.Add((Control)(object)txtFileName);
		((Control)GroupBox4).Controls.Add((Control)(object)txtGUID);
		((Control)GroupBox4).Controls.Add((Control)(object)txtCTSSDesc);
		((Control)GroupBox4).Controls.Add((Control)(object)txtCTSSName);
		((Control)GroupBox4).Controls.Add((Control)(object)Label4);
		((Control)GroupBox4).Controls.Add((Control)(object)Label3);
		((Control)GroupBox4).Controls.Add((Control)(object)Label2);
		((Control)GroupBox4).Controls.Add((Control)(object)Label1);
		((Control)GroupBox4).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)GroupBox4).ForeColor = SystemColors.ControlText;
		GroupBox groupBox5 = GroupBox4;
		location = new Point(88, 80);
		((Control)groupBox5).Location = location;
		((Control)GroupBox4).Name = "GroupBox4";
		((Control)GroupBox4).RightToLeft = (RightToLeft)0;
		GroupBox groupBox6 = GroupBox4;
		size = new Size(457, 352);
		((Control)groupBox6).Size = size;
		((Control)GroupBox4).TabIndex = 68;
		GroupBox4.TabStop = false;
		GroupBox4.Text = "Details About Your Selection:";
		((Control)GroupBox4).Visible = false;
		((Control)Label14).BackColor = SystemColors.InactiveCaptionText;
		((Control)Label14).Cursor = Cursors.Default;
		((Control)Label14).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label14).ForeColor = SystemColors.ControlText;
		Label label19 = Label14;
		location = new Point(296, 176);
		((Control)label19).Location = location;
		((Control)Label14).Name = "Label14";
		((Control)Label14).RightToLeft = (RightToLeft)0;
		Label label20 = Label14;
		size = new Size(137, 17);
		((Control)label20).Size = size;
		((Control)Label14).TabIndex = 25;
		((Control)Label14).Text = "Catalog Sorts:";
		lstCategories.ItemHeight = 14;
		ListBox obj89 = lstCategories;
		location = new Point(296, 200);
		((Control)obj89).Location = location;
		((Control)lstCategories).Name = "lstCategories";
		ListBox obj90 = lstCategories;
		size = new Size(152, 88);
		((Control)obj90).Size = size;
		lstCategories.Sorted = true;
		((Control)lstCategories).TabIndex = 24;
		((Control)Label13).BackColor = SystemColors.InactiveCaptionText;
		((Control)Label13).Cursor = Cursors.Default;
		((Control)Label13).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label13).ForeColor = SystemColors.ControlText;
		Label label21 = Label13;
		location = new Point(296, 8);
		((Control)label21).Location = location;
		((Control)Label13).Name = "Label13";
		((Control)Label13).RightToLeft = (RightToLeft)0;
		Label label22 = Label13;
		size = new Size(137, 17);
		((Control)label22).Size = size;
		((Control)Label13).TabIndex = 23;
		((Control)Label13).Text = "Preview:";
		PictureBox pictureBox3 = PictureBox1;
		location = new Point(296, 24);
		((Control)pictureBox3).Location = location;
		((Control)PictureBox1).Name = "PictureBox1";
		PictureBox pictureBox4 = PictureBox1;
		size = new Size(152, 152);
		((Control)pictureBox4).Size = size;
		PictureBox1.TabIndex = 22;
		PictureBox1.TabStop = false;
		((Control)cmdCancel).BackColor = SystemColors.Control;
		((Control)cmdCancel).Cursor = Cursors.Default;
		((Control)cmdCancel).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdCancel).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdCancel).Image = (Image)resourceManager.GetObject("cmdCancel.Image");
		Button obj91 = cmdCancel;
		location = new Point(240, 296);
		((Control)obj91).Location = location;
		((Control)cmdCancel).Name = "cmdCancel";
		((Control)cmdCancel).RightToLeft = (RightToLeft)0;
		Button obj92 = cmdCancel;
		size = new Size(44, 44);
		((Control)obj92).Size = size;
		((Control)cmdCancel).TabIndex = 21;
		ToolTip1.SetToolTip((Control)(object)cmdCancel, "Cancel");
		((Control)cmdAddItem).BackColor = SystemColors.Control;
		((Control)cmdAddItem).Cursor = Cursors.Default;
		((Control)cmdAddItem).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdAddItem).ForeColor = SystemColors.ControlText;
		((ButtonBase)cmdAddItem).Image = (Image)resourceManager.GetObject("cmdAddItem.Image");
		Button obj93 = cmdAddItem;
		location = new Point(168, 296);
		((Control)obj93).Location = location;
		((Control)cmdAddItem).Name = "cmdAddItem";
		((Control)cmdAddItem).RightToLeft = (RightToLeft)0;
		Button obj94 = cmdAddItem;
		size = new Size(44, 44);
		((Control)obj94).Size = size;
		((Control)cmdAddItem).TabIndex = 20;
		ToolTip1.SetToolTip((Control)(object)cmdAddItem, "OK");
		txtGroup.AcceptsReturn = true;
		((TextBoxBase)txtGroup).AutoSize = false;
		((TextBoxBase)txtGroup).BackColor = SystemColors.Window;
		((Control)txtGroup).Cursor = Cursors.IBeam;
		((Control)txtGroup).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((TextBoxBase)txtGroup).ForeColor = SystemColors.WindowText;
		TextBox obj95 = txtGroup;
		location = new Point(152, 32);
		((Control)obj95).Location = location;
		((TextBoxBase)txtGroup).MaxLength = 0;
		((Control)txtGroup).Name = "txtGroup";
		((TextBoxBase)txtGroup).ReadOnly = true;
		((Control)txtGroup).RightToLeft = (RightToLeft)0;
		TextBox obj96 = txtGroup;
		size = new Size(136, 19);
		((Control)obj96).Size = size;
		((Control)txtGroup).TabIndex = 19;
		txtGroup.Text = "";
		txtFileName.AcceptsReturn = true;
		((TextBoxBase)txtFileName).AutoSize = false;
		((TextBoxBase)txtFileName).BackColor = SystemColors.Window;
		((Control)txtFileName).Cursor = Cursors.IBeam;
		((Control)txtFileName).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((TextBoxBase)txtFileName).ForeColor = SystemColors.WindowText;
		TextBox obj97 = txtFileName;
		location = new Point(8, 72);
		((Control)obj97).Location = location;
		((TextBoxBase)txtFileName).MaxLength = 0;
		((Control)txtFileName).Name = "txtFileName";
		((TextBoxBase)txtFileName).ReadOnly = true;
		((Control)txtFileName).RightToLeft = (RightToLeft)0;
		TextBox obj98 = txtFileName;
		size = new Size(280, 19);
		((Control)obj98).Size = size;
		((Control)txtFileName).TabIndex = 17;
		txtFileName.Text = "";
		txtGUID.AcceptsReturn = true;
		((TextBoxBase)txtGUID).AutoSize = false;
		((TextBoxBase)txtGUID).BackColor = SystemColors.Window;
		((Control)txtGUID).Cursor = Cursors.IBeam;
		((Control)txtGUID).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((TextBoxBase)txtGUID).ForeColor = SystemColors.WindowText;
		TextBox obj99 = txtGUID;
		location = new Point(8, 32);
		((Control)obj99).Location = location;
		((TextBoxBase)txtGUID).MaxLength = 0;
		((Control)txtGUID).Name = "txtGUID";
		((TextBoxBase)txtGUID).ReadOnly = true;
		((Control)txtGUID).RightToLeft = (RightToLeft)0;
		TextBox obj100 = txtGUID;
		size = new Size(136, 19);
		((Control)obj100).Size = size;
		((Control)txtGUID).TabIndex = 15;
		txtGUID.Text = "";
		txtCTSSDesc.AcceptsReturn = true;
		((TextBoxBase)txtCTSSDesc).AutoSize = false;
		((TextBoxBase)txtCTSSDesc).BackColor = SystemColors.Window;
		((Control)txtCTSSDesc).Cursor = Cursors.IBeam;
		((Control)txtCTSSDesc).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((TextBoxBase)txtCTSSDesc).ForeColor = SystemColors.WindowText;
		TextBox obj101 = txtCTSSDesc;
		location = new Point(8, 136);
		((Control)obj101).Location = location;
		((TextBoxBase)txtCTSSDesc).MaxLength = 0;
		((TextBoxBase)txtCTSSDesc).Multiline = true;
		((Control)txtCTSSDesc).Name = "txtCTSSDesc";
		((TextBoxBase)txtCTSSDesc).ReadOnly = true;
		((Control)txtCTSSDesc).RightToLeft = (RightToLeft)0;
		TextBox obj102 = txtCTSSDesc;
		size = new Size(281, 153);
		((Control)obj102).Size = size;
		((Control)txtCTSSDesc).TabIndex = 13;
		txtCTSSDesc.Text = "";
		txtCTSSName.AcceptsReturn = true;
		((TextBoxBase)txtCTSSName).AutoSize = false;
		((TextBoxBase)txtCTSSName).BackColor = SystemColors.Window;
		((Control)txtCTSSName).Cursor = Cursors.IBeam;
		((Control)txtCTSSName).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((TextBoxBase)txtCTSSName).ForeColor = SystemColors.WindowText;
		TextBox obj103 = txtCTSSName;
		location = new Point(8, 112);
		((Control)obj103).Location = location;
		((TextBoxBase)txtCTSSName).MaxLength = 0;
		((Control)txtCTSSName).Name = "txtCTSSName";
		((TextBoxBase)txtCTSSName).ReadOnly = true;
		((Control)txtCTSSName).RightToLeft = (RightToLeft)0;
		TextBox obj104 = txtCTSSName;
		size = new Size(281, 19);
		((Control)obj104).Size = size;
		((Control)txtCTSSName).TabIndex = 12;
		txtCTSSName.Text = "";
		((Control)Label4).BackColor = SystemColors.InactiveCaptionText;
		((Control)Label4).Cursor = Cursors.Default;
		((Control)Label4).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label4).ForeColor = SystemColors.ControlText;
		Label label23 = Label4;
		location = new Point(152, 16);
		((Control)label23).Location = location;
		((Control)Label4).Name = "Label4";
		((Control)Label4).RightToLeft = (RightToLeft)0;
		Label label24 = Label4;
		size = new Size(136, 17);
		((Control)label24).Size = size;
		((Control)Label4).TabIndex = 18;
		((Control)Label4).Text = "Group:";
		((Control)Label3).BackColor = SystemColors.InactiveCaptionText;
		((Control)Label3).Cursor = Cursors.Default;
		((Control)Label3).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label3).ForeColor = SystemColors.ControlText;
		Label label25 = Label3;
		location = new Point(8, 56);
		((Control)label25).Location = location;
		((Control)Label3).Name = "Label3";
		((Control)Label3).RightToLeft = (RightToLeft)0;
		Label label26 = Label3;
		size = new Size(280, 17);
		((Control)label26).Size = size;
		((Control)Label3).TabIndex = 16;
		((Control)Label3).Text = "Filename:";
		((Control)Label2).BackColor = SystemColors.InactiveCaptionText;
		((Control)Label2).Cursor = Cursors.Default;
		((Control)Label2).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2).ForeColor = SystemColors.ControlText;
		Label label27 = Label2;
		location = new Point(8, 16);
		((Control)label27).Location = location;
		((Control)Label2).Name = "Label2";
		((Control)Label2).RightToLeft = (RightToLeft)0;
		Label label28 = Label2;
		size = new Size(137, 17);
		((Control)label28).Size = size;
		((Control)Label2).TabIndex = 14;
		((Control)Label2).Text = "GUID:";
		((Control)Label1).BackColor = SystemColors.InactiveCaptionText;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		Label label29 = Label1;
		location = new Point(8, 96);
		((Control)label29).Location = location;
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		Label label30 = Label1;
		size = new Size(280, 17);
		((Control)label30).Size = size;
		((Control)Label1).TabIndex = 11;
		((Control)Label1).Text = "Catalog Description:";
		lstListOfItems.BackColor = SystemColors.Window;
		((Control)lstListOfItems).Cursor = Cursors.Default;
		((Control)lstListOfItems).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		lstListOfItems.ForeColor = SystemColors.WindowText;
		lstListOfItems.ItemHeight = 14;
		ListBox obj105 = lstListOfItems;
		location = new Point(96, 167);
		((Control)obj105).Location = location;
		((Control)lstListOfItems).Name = "lstListOfItems";
		lstListOfItems.RightToLeft = (RightToLeft)0;
		ListBox obj106 = lstListOfItems;
		size = new Size(441, 214);
		((Control)obj106).Size = size;
		((Control)lstListOfItems).TabIndex = 61;
		ListBox obj107 = lstABC1;
		location = new Point(8, 112);
		((Control)obj107).Location = location;
		((Control)lstABC1).Name = "lstABC1";
		ListBox obj108 = lstABC1;
		size = new Size(64, 134);
		((Control)obj108).Size = size;
		((Control)lstABC1).TabIndex = 96;
		((Control)lstABC1).Visible = false;
		ListBox obj109 = lstRecursive;
		location = new Point(8, 263);
		((Control)obj109).Location = location;
		((Control)lstRecursive).Name = "lstRecursive";
		ListBox obj110 = lstRecursive;
		size = new Size(64, 147);
		((Control)obj110).Size = size;
		((Control)lstRecursive).TabIndex = 98;
		((Control)lstRecursive).Visible = false;
		((FileDialog)cdlSaveFile).DefaultExt = "package";
		((Control)GroupBox3).Controls.Add((Control)(object)lblProgress);
		((Control)GroupBox3).Controls.Add((Control)(object)ProgressBar1);
		((Control)GroupBox3).Controls.Add((Control)(object)lblProgressDesc);
		GroupBox groupBox7 = GroupBox3;
		location = new Point(192, 0);
		((Control)groupBox7).Location = location;
		((Control)GroupBox3).Name = "GroupBox3";
		GroupBox groupBox8 = GroupBox3;
		size = new Size(256, 88);
		((Control)groupBox8).Size = size;
		((Control)GroupBox3).TabIndex = 100;
		GroupBox3.TabStop = false;
		GroupBox3.Text = "Working...";
		((Control)GroupBox3).Visible = false;
		Label obj111 = lblProgress;
		location = new Point(80, 32);
		((Control)obj111).Location = location;
		((Control)lblProgress).Name = "lblProgress";
		Label obj112 = lblProgress;
		size = new Size(100, 16);
		((Control)obj112).Size = size;
		((Control)lblProgress).TabIndex = 2;
		lblProgress.TextAlign = (ContentAlignment)2;
		ProgressBar progressBar = ProgressBar1;
		location = new Point(8, 56);
		((Control)progressBar).Location = location;
		ProgressBar1.Maximum = 0;
		((Control)ProgressBar1).Name = "ProgressBar1";
		ProgressBar progressBar2 = ProgressBar1;
		size = new Size(240, 24);
		((Control)progressBar2).Size = size;
		ProgressBar1.Step = 1;
		((Control)ProgressBar1).TabIndex = 1;
		Label obj113 = lblProgressDesc;
		location = new Point(8, 16);
		((Control)obj113).Location = location;
		((Control)lblProgressDesc).Name = "lblProgressDesc";
		Label obj114 = lblProgressDesc;
		size = new Size(240, 16);
		((Control)obj114).Size = size;
		((Control)lblProgressDesc).TabIndex = 0;
		((Control)lblProgressDesc).Text = "Now Reading Resource:";
		lblProgressDesc.TextAlign = (ContentAlignment)2;
		StatusBar statusBar = StatusBar1;
		location = new Point(0, 528);
		((Control)statusBar).Location = location;
		((Control)StatusBar1).Name = "StatusBar1";
		StatusBar1.Panels.AddRange((StatusBarPanel[])(object)new StatusBarPanel[1] { Panel1 });
		StatusBar1.ShowPanels = true;
		StatusBar statusBar2 = StatusBar1;
		size = new Size(640, 22);
		((Control)statusBar2).Size = size;
		((Control)StatusBar1).TabIndex = 101;
		Panel1.Alignment = (HorizontalAlignment)2;
		Panel1.Text = "Status";
		Panel1.Width = 632;
		ListBox obj115 = lstBatchFileList;
		location = new Point(80, 464);
		((Control)obj115).Location = location;
		((Control)lstBatchFileList).Name = "lstBatchFileList";
		ListBox obj116 = lstBatchFileList;
		size = new Size(472, 56);
		((Control)obj116).Size = size;
		((Control)lstBatchFileList).TabIndex = 102;
		((Control)lstBatchFileList).Visible = false;
		size = new Size(5, 13);
		((Form)this).AutoScaleBaseSize = size;
		((Control)this).BackgroundImage = (Image)resourceManager.GetObject("$this.BackgroundImage");
		size = new Size(640, 550);
		((Form)this).ClientSize = size;
		((Control)this).Controls.Add((Control)(object)lstBatchFileList);
		((Control)this).Controls.Add((Control)(object)StatusBar1);
		((Control)this).Controls.Add((Control)(object)btnDEBUG);
		((Control)this).Controls.Add((Control)(object)cmdAlphaSort);
		((Control)this).Controls.Add((Control)(object)cmdAbout);
		((Control)this).Controls.Add((Control)(object)cmdOptions);
		((Control)this).Controls.Add((Control)(object)txtImgPath);
		((Control)this).Controls.Add((Control)(object)lstDatFileNames);
		((Control)this).Controls.Add((Control)(object)txtCollID);
		((Control)this).Controls.Add((Control)(object)txtPackagePath);
		((Control)this).Controls.Add((Control)(object)lstInstance2);
		((Control)this).Controls.Add((Control)(object)lstInstance);
		((Control)this).Controls.Add((Control)(object)lstGroups);
		((Control)this).Controls.Add((Control)(object)lstResources);
		((Control)this).Controls.Add((Control)(object)txtIndexSize);
		((Control)this).Controls.Add((Control)(object)txtResourceCount);
		((Control)this).Controls.Add((Control)(object)lstSize);
		((Control)this).Controls.Add((Control)(object)lstOffset);
		((Control)this).Controls.Add((Control)(object)cmbCollType);
		((Control)this).Controls.Add((Control)(object)cmdLoadPic);
		((Control)this).Controls.Add((Control)(object)Picture1);
		((Control)this).Controls.Add((Control)(object)txtCollName);
		((Control)this).Controls.Add((Control)(object)cmdMoveDown);
		((Control)this).Controls.Add((Control)(object)cmdRemoveItem);
		((Control)this).Controls.Add((Control)(object)cmdMoveUp);
		((Control)this).Controls.Add((Control)(object)cmdBackUpColl);
		((Control)this).Controls.Add((Control)(object)cmdEditColl);
		((Control)this).Controls.Add((Control)(object)cmdMakeNewColl);
		((Control)this).Controls.Add((Control)(object)cmdSaveColl);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdAddWallFloor);
		((Control)this).Controls.Add((Control)(object)cmdBatchAdd);
		((Control)this).Controls.Add((Control)(object)Command1);
		((Control)this).Controls.Add((Control)(object)Label7);
		((Control)this).Controls.Add((Control)(object)Label6);
		((Control)this).Controls.Add((Control)(object)Label5);
		((Control)this).Controls.Add((Control)(object)lstListOfItems);
		((Control)this).Controls.Add((Control)(object)lstABC1);
		((Control)this).Controls.Add((Control)(object)lstRecursive);
		((Control)this).Controls.Add((Control)(object)GroupBox3);
		((Control)this).Controls.Add((Control)(object)GroupBox1);
		((Control)this).Controls.Add((Control)(object)GroupBox2);
		((Control)this).Controls.Add((Control)(object)GroupBox4);
		((Form)this).Icon = (Icon)resourceManager.GetObject("$this.Icon");
		((Control)this).Name = "frmMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Sims 2 Collection Creator - All Your Sim Are Belong To Us!";
		((Control)GroupBox2).ResumeLayout(false);
		((Control)GroupBox1).ResumeLayout(false);
		((Control)GroupBox4).ResumeLayout(false);
		((Control)GroupBox3).ResumeLayout(false);
		((ISupportInitialize)Panel1).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void cmdAddItem_Click(object eventSender, EventArgs eventArgs)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		string text = txtGroup.Text;
		text = ((StringType.StrCmp(text, "FFFFFFFF", false) != 0) ? text : "00000000");
		if (StringType.StrCmp(txtGUID.Text, "", false) == 0)
		{
			Interaction.MsgBox((object)"Sorry, you can't add an item that doesn't have a GUID. Perhaps this is a recolor and not a mesh? You can only add meshes you know!", (MsgBoxStyle)48, (object)"Hey, it's an error!");
			ShowUI(4L);
			return;
		}
		string text2 = txtGUID.Text;
		string text3 = "69DA3F9F";
		string text4 = "00000000";
		string text5 = txtCTSSName.Text;
		lstListOfItems.Items.Add((object)("Object - " + text5 + ": " + text3 + " - " + text4 + " - " + text + " - " + text2));
		string text6 = Common.Translate("0x" + text2, Application.StartupPath + "/UserObjectList.txt", 1L, 2L, ';');
		if (StringType.StrCmp(text6, "No Match Found", false) == 0)
		{
			Common.AppendToFile("0x" + text + ";0x" + text2 + ";" + text5 + ";;", Application.StartupPath + "/UserObjectList.txt");
		}
		ShowUI(4L);
		ChangeSta("Single object add completed successfully.");
		OldStaText = "Single object add completed successfully";
	}

	private void cmdExit_Click(object eventSender, EventArgs eventArgs)
	{
		IOTools.killFiles(Application.StartupPath + "/", "*.dat");
		((Form)this).Close();
		ProjectData.EndApp();
	}

	private void Command1_Click(object eventSender, EventArgs eventArgs)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		txtPackagePath.Text = "";
		((FileDialog)cdlOpenFile).Title = "Open a Package File";
		((FileDialog)cdlOpenFile).FileName = "";
		((FileDialog)cdlOpenFile).DefaultExt = "package";
		((FileDialog)cdlOpenFile).Filter = "Package Files (*.package)|*.package|All Files (*.*)|*.*";
		((CommonDialog)cdlOpenFile).ShowDialog();
		txtPackagePath.Text = ((FileDialog)cdlOpenFile).FileName;
		string text = Strings.LCase(txtPackagePath.Text);
		string text2 = ".package";
		long num = Strings.InStr(text, text2, (CompareMethod)1);
		if (num == 0L)
		{
			ChangeSta("File Not Loaded. Error: Doesn't appear to be a package file.");
			OldStaText = "File Not Loaded. Error: Doesn't appear to be a package file.";
		}
		else if (StringType.StrCmp(txtPackagePath.Text, "", false) != 0)
		{
			HideUI(4L);
			ChangeSta("File Successfully Loaded!");
			OldStaText = "File Successfully Loaded!";
			txtFileName.Text = ((FileDialog)cdlOpenFile).FileName;
			string fileName = ((FileDialog)cdlOpenFile).FileName;
			txtPackagePath.Text = fileName;
			FindResource(fileName);
			HandleOBJD(fileName, 1);
			HandleCTSS(fileName);
			PullThumbnail(fileName);
		}
	}

	public object FindResource(string PackagePath)
	{
		lstResources.Items.Clear();
		lstInstance.Items.Clear();
		lstInstance2.Items.Clear();
		lstOffset.Items.Clear();
		lstSize.Items.Clear();
		lstGroups.Items.Clear();
		txtResourceCount.Text = "";
		txtResourceCount.Text = StringType.FromLong(IO.FetchResourceCount(PackagePath));
		checked
		{
			if (DoubleType.FromString(txtResourceCount.Text) != 0.0)
			{
				txtIndexSize.Text = "";
				txtIndexSize.Text = StringType.FromLong(IO.FetchIndexSize(PackagePath));
				string[] array = IO.FetchResourceArray(PackagePath);
				for (long num = 0L; num < array.Length; num++)
				{
					string[] array2 = array[(int)num].Split(new char[1] { ',' });
					lstResources.Items.Add((object)array2[0]);
					lstGroups.Items.Add((object)array2[1]);
					lstInstance.Items.Add((object)array2[2]);
					if (array2.Length == 6)
					{
						lstInstance2.Items.Add((object)array2[3]);
						lstOffset.Items.Add((object)CCB.Math.ToDec(array2[4]));
						lstSize.Items.Add((object)CCB.Math.ToDec(array2[5]));
					}
					else
					{
						lstOffset.Items.Add((object)CCB.Math.ToDec(array2[3]));
						lstSize.Items.Add((object)CCB.Math.ToDec(array2[4]));
					}
				}
			}
			else
			{
				lstResources.Items.Clear();
				lstInstance.Items.Clear();
				lstInstance2.Items.Clear();
				lstOffset.Items.Clear();
				lstSize.Items.Clear();
				lstGroups.Items.Clear();
			}
			object result = default(object);
			return result;
		}
	}

	public object HandleCTSS(string PackagePath)
	{
		string text = "43545353";
		txtCTSSName.Text = "";
		txtCTSSDesc.Text = "";
		int count = lstResources.Items.Count;
		int i = 0;
		IOTools.killFiles(Application.StartupPath + "/", "*.dat");
		checked
		{
			for (; i != count; i++)
			{
				if (ObjectType.ObjTst((object)text, lstResources.Items[i], false) == 0)
				{
					long num = lstOffset.Items.Count;
					num = LongType.FromObject(lstOffset.Items[i]);
					long num2 = LongType.FromObject(lstSize.Items[i]);
					long num3 = i + 1;
					IO.DumpResourceData(PackagePath, (int)num, (int)num3, (int)num2, text, Application.StartupPath);
					break;
				}
			}
			string[] array = IOTools.GetsFilesNoDir(Application.StartupPath + "/", "*.dat");
			for (long num4 = 0L; num4 < array.Length; num4++)
			{
				DumpDesc(array[(int)num4], 1L);
			}
			object result = default(object);
			return result;
		}
	}

	public object DumpDesc(string CTSSDataPath, long Action)
	{
		int try0000_dispatch = -1;
		FileStream fileStream = default(FileStream);
		BinaryReader binaryReader = default(BinaryReader);
		int num2 = default(int);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
				{
					fileStream = new FileStream(Application.StartupPath + "/" + IO.Decompress(CTSSDataPath, 1, Application.StartupPath), FileMode.Open, FileAccess.ReadWrite);
					binaryReader = new BinaryReader(fileStream);
					int num = 0;
					binaryReader.BaseStream.Position = 69L;
					ProjectData.ClearProjectError();
					num2 = 1;
					while (true)
					{
						byte b = binaryReader.ReadByte();
						string text = ((b != 10) ? StringType.FromChar(Strings.Chr((int)b)) : "\r\n");
						checked
						{
							if (b == 0)
							{
								num++;
								binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + 1L;
								continue;
							}
							switch (num)
							{
							case 1:
								continue;
							case 0:
								txtCTSSName.Text += text;
								if (Action == 2L)
								{
									txtCollName.Text += text;
								}
								continue;
							case 2:
								txtCTSSDesc.Text += text;
								continue;
							}
							break;
						}
					}
					break;
				}
				case 285:
					break;
				}
				binaryReader.Close();
				fileStream.Close();
			}
			catch (object obj) when ((obj is Exception && num2 != 0 && num3 == 0) ? true : false)
			{
				Exception obj2 = (Exception)obj;
				ProjectData.SetProjectError(obj2);
				Exception ex = obj2;
				if (num3 != 0)
				{
					break;
				}
				num3 = -1;
				switch (num2)
				{
				case 1:
					try0000_dispatch = 285;
					break;
				default:
					throw;
				}
				continue;
			}
			break;
		}
		if (num3 != 0)
		{
			ProjectData.ClearProjectError();
		}
		object result = default(object);
		return result;
	}

	public object HandleOBJD(string PackagePath, int Type)
	{
		if (Type == 1)
		{
			lstCategories.Items.Clear();
		}
		string text = "4F424A44";
		txtGUID.Text = "";
		int count = lstResources.Items.Count;
		int num = 0;
		IOTools.killFiles(Application.StartupPath + "/", "*.dat");
		checked
		{
			while (num != count)
			{
				if (ObjectType.ObjTst((object)text, lstResources.Items[num], false) == 0)
				{
					long num2 = lstOffset.Items.Count;
					num2 = LongType.FromObject(lstOffset.Items[num]);
					long num3 = LongType.FromObject(lstSize.Items[num]);
					string text2 = StringType.FromObject(lstGroups.Items[num]);
					long num4 = num + 1;
					txtGroup.Text = text2;
					IO.DumpResourceData(PackagePath, (int)num2, (int)num4, (int)num3, text, Application.StartupPath);
					num++;
				}
				else
				{
					num++;
				}
			}
			string[] array = IOTools.GetsFilesNoDir(Application.StartupPath + "/", "*.dat");
			long num5 = 0L;
			while (num5 < array.Length)
			{
				if (OBJD.ValidPrimaryOBJD(Application.StartupPath + "/" + IO.Decompress(array[(int)num5], (int)num5, Application.StartupPath)))
				{
					string[] array2 = OBJD.FetchCatalogCategories(Application.StartupPath + "/" + IO.Decompress(array[(int)num5], (int)num5, Application.StartupPath));
					for (long num6 = 0L; num6 < array2.Length; num6++)
					{
						switch (Type)
						{
						case 1:
							if (lstCategories.FindString(array2[(int)num6]) == -1)
							{
								lstCategories.Items.Add((object)array2[(int)num6]);
							}
							break;
						case 2:
							if (lstBatchCategories.FindString(array2[(int)num6]) == -1)
							{
								lstBatchCategories.Items.Add((object)array2[(int)num6]);
							}
							break;
						}
					}
					txtGUID.Text = OBJD.FetchGUID(Application.StartupPath + "/" + IO.Decompress(array[(int)num5], (int)num5, Application.StartupPath));
					num5++;
				}
				else
				{
					num5++;
				}
			}
			object result = default(object);
			return result;
		}
	}

	private void cmdCancel_Click(object sender, EventArgs e)
	{
		ChangeSta("Single object add canceled.");
		OldStaText = "Single object add canceled.";
		ShowUI(4L);
	}

	private void cmdMoveUp_Click(object sender, EventArgs e)
	{
		if (lstListOfItems.SelectedIndex == -1)
		{
			if (!chkWarningOff.Checked)
			{
				ChangeSta("You must select an item to move first or else the program will a splode!");
				OldStaText = "You must select an item to move first or else the program will a splode!";
			}
			return;
		}
		if (lstListOfItems.SelectedIndex == 0)
		{
			if (!chkWarningOff.Checked)
			{
				ChangeSta("Item cannot be moved up anymore.");
				OldStaText = "Item cannot be moved up anymore.";
			}
			return;
		}
		long num = lstListOfItems.SelectedIndex;
		checked
		{
			string text = StringType.FromObject(lstListOfItems.Items[(int)num]);
			string text2 = StringType.FromObject(lstListOfItems.Items[(int)(num - 1L)]);
			lstListOfItems.Items[(int)num] = text2;
			lstListOfItems.Items[(int)(num - 1L)] = text;
			lstListOfItems.SelectedIndex = (int)(num - 1L);
			ChangeSta("Up, up and away!");
			OldStaText = "Up, up and away!";
		}
	}

	private void cmdMoveDown_Click(object sender, EventArgs e)
	{
		if (lstListOfItems.SelectedIndex == -1)
		{
			if (!chkWarningOff.Checked)
			{
				ChangeSta("You must select an item to move first or else the program will a splode!");
				OldStaText = "You must select an item to move first or else the program will a splode!";
			}
			return;
		}
		checked
		{
			if (lstListOfItems.SelectedIndex == lstListOfItems.Items.Count - 1)
			{
				if (!chkWarningOff.Checked)
				{
					ChangeSta("Item cannot be moved down anymore.");
					OldStaText = "Item cannot be moved down anymore.";
				}
				return;
			}
			long num = lstListOfItems.SelectedIndex;
			string text = StringType.FromObject(lstListOfItems.Items[(int)num]);
			string text2 = StringType.FromObject(lstListOfItems.Items[(int)(num + 1L)]);
			lstListOfItems.Items[(int)num] = text2;
			lstListOfItems.Items[(int)(num + 1L)] = text;
			lstListOfItems.SelectedIndex = (int)(num + 1L);
			ChangeSta("Oh no... we're going down!");
			OldStaText = "Oh no... we're going down!";
		}
	}

	private void cmdRemoveItem_Click(object sender, EventArgs e)
	{
		if (lstListOfItems.SelectedIndex == -1)
		{
			ChangeSta("You must select an item to remove first or else the program will a splode!");
			OldStaText = "You must select an item to remove first or else the program will a splode!";
		}
		else if (lstListOfItems.Items.Count != 1)
		{
			lstListOfItems.Items.RemoveAt(lstListOfItems.SelectedIndex);
			lstListOfItems.SelectedIndex = 0;
			ChangeSta("Item removed from collection list.");
			OldStaText = "Item removed from collection list.";
		}
	}

	private void cmdMakeNewColl_Click(object sender, EventArgs e)
	{
		((Control)Command1).Enabled = true;
		((Control)cmdSaveColl).Enabled = true;
		((Control)cmdMoveUp).Enabled = true;
		((Control)cmdRemoveItem).Enabled = true;
		((Control)cmdMoveDown).Enabled = true;
		((Control)cmdBatchAdd).Enabled = true;
		Random random = new Random();
		long num = random.Next(4369, 65535);
		txtCollID.Text = Conversion.Hex(num);
		num = random.Next(4369, 65535);
		txtCollID.Text += Conversion.Hex(num);
		lstListOfItems.Items.Clear();
		cmbCollType.Text = "";
		if (!chkWarningOff.Checked)
		{
			ChangeSta("You're all set to make a new collection!");
			OldStaText = "You're all set to make a new collection!";
		}
	}

	private void lstInstance2_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Interaction.MsgBox((object)"Hey! Don't touch me! I'm here for debugging purposes only!", (MsgBoxStyle)48, (object)"Don't touch me!");
	}

	private void lstInstance_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Interaction.MsgBox((object)"Hey! Don't touch me! I'm here for debugging purposes only!", (MsgBoxStyle)48, (object)"Don't touch me!");
	}

	private void lstOffset_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Interaction.MsgBox((object)"Hey! Don't touch me! I'm here for debugging purposes only!", (MsgBoxStyle)48, (object)"Don't touch me!");
	}

	private void lstSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Interaction.MsgBox((object)"Hey! Don't touch me! I'm here for debugging purposes only!", (MsgBoxStyle)48, (object)"Don't touch me!");
	}

	private void lstResources_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Interaction.MsgBox((object)"Hey! Don't touch me! I'm here for debugging purposes only!", (MsgBoxStyle)48, (object)"Don't touch me!");
	}

	private void lstGroups_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Interaction.MsgBox((object)"Hey! Don't touch me! I'm here for debugging purposes only!", (MsgBoxStyle)48, (object)"Don't touch me!");
	}

	private void cmdLoadPic_Click(object sender, EventArgs e)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		if (!chkWarningOff.Checked)
		{
			ChangeSta("The program comes with sample icons. You can use your own, but they MUST be in jpg or bmp format.");
			OldStaText = "The program comes with sample icons. You can use your own, but they MUST be in jpg or bmp format.";
		}
		txtImgPath.Text = "";
		((FileDialog)cdlOpenFile).FileName = "";
		((FileDialog)cdlOpenFile).Title = "Choose A Collection Icon";
		((FileDialog)cdlOpenFile).Filter = "JPG Images (*.jpg)|*.jpg|BMP Images (*.bmp)|*.bmp|All Files (*.*)|*.*";
		((FileDialog)cdlOpenFile).InitialDirectory = Application.StartupPath + "/Sample Icons/";
		((CommonDialog)cdlOpenFile).ShowDialog();
		((FileDialog)cdlOpenFile).InitialDirectory = "";
		txtImgPath.Text = ((FileDialog)cdlOpenFile).FileName;
		if (StringType.StrCmp(txtImgPath.Text, "", false) == 0)
		{
			return;
		}
		string text = Strings.LCase(((FileDialog)cdlOpenFile).FileName);
		string text2 = ".jpg";
		long num = Strings.InStr(text, text2, (CompareMethod)1);
		if (num == 0L)
		{
			text = Strings.LCase(((FileDialog)cdlOpenFile).FileName);
			text2 = ".bmp";
			num = Strings.InStr(text, text2, (CompareMethod)1);
			if (num == 0L)
			{
				text = Strings.LCase(((FileDialog)cdlOpenFile).FileName);
				text2 = ".tga";
				num = Strings.InStr(text, text2, (CompareMethod)1);
				if (num == 0L)
				{
					ChangeSta("Image Not Loaded: This is not a jpg, bmp, or tga image!");
					OldStaText = "Image Not Loaded: This is not a jpg, bmp, or tga image!";
				}
				else
				{
					Picture1.Image = Image.FromFile(txtImgPath.Text);
				}
			}
			else
			{
				Picture1.Image = Image.FromFile(txtImgPath.Text);
			}
		}
		else
		{
			Picture1.Image = Image.FromFile(txtImgPath.Text);
		}
	}

	private void cmdSaveColl_Click(object sender, EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		if (lstListOfItems.Items.Count == 0)
		{
			ChangeSta("Collection NOT Saved: You can't make a collection with no items in it.");
			OldStaText = "Collection NOT Saved: You can't make a collection with no items in it.";
			return;
		}
		if (StringType.StrCmp(txtImgPath.Text, "", false) == 0)
		{
			Interaction.MsgBox((object)"Hey! You have to choose an icon for the collection before you can save!", (MsgBoxStyle)48, (object)"Danger Will Robinson, Danger!");
			return;
		}
		if (StringType.StrCmp(txtCollName.Text, "", false) == 0)
		{
			Interaction.MsgBox((object)"Hey! You have to name the collection before you can save it!", (MsgBoxStyle)48, (object)"404! Name of your collection not found!");
			return;
		}
		if (StringType.StrCmp(cmbCollType.Text, "", false) == 0)
		{
			Interaction.MsgBox((object)"Hey! You have to choose what type of collection you're making! (Residential or Commercial!)", (MsgBoxStyle)48, (object)"I've given up all hope...");
			return;
		}
		((FileDialog)cdlSaveFile).InitialDirectory = txtCollDir.Text;
		((FileDialog)cdlSaveFile).Title = "Save Your Collection File";
		((FileDialog)cdlSaveFile).FileName = "";
		((FileDialog)cdlSaveFile).DefaultExt = "package";
		((FileDialog)cdlSaveFile).Filter = "Package Files (*.package)|*.package|All Files (*.*)|*.*";
		((FileDialog)cdlSaveFile).AddExtension = true;
		((CommonDialog)cdlSaveFile).ShowDialog();
		((FileDialog)cdlSaveFile).InitialDirectory = "";
		txtPackagePath.Text = ((FileDialog)cdlSaveFile).FileName;
		short num = -1;
		string text = FileSystem.Dir(Application.StartupPath + "/*.dat", (FileAttribute)0);
		checked
		{
			string[] array = default(string[]);
			while (StringType.StrCmp(text, (string)null, false) != 0)
			{
				num++;
				array = (string[])Utils.CopyArray((Array)array, (Array)new string[num + 1]);
				array[num] = text;
				FileSystem.Kill(Application.StartupPath + "/" + array[num]);
				text = FileSystem.Dir();
			}
			if (StringType.StrCmp(txtPackagePath.Text, "", false) != 0)
			{
				if (File.Exists(((FileDialog)cdlSaveFile).FileName))
				{
					FileSystem.Kill(((FileDialog)cdlSaveFile).FileName);
				}
				lstDatFileNames.Items.Clear();
				lstGroups.Items.Clear();
				lstResources.Items.Clear();
				lstSize.Items.Clear();
				lstOffset.Items.Clear();
				lstInstance.Items.Clear();
				lstInstance2.Items.Clear();
				txtResourceCount.Text = "";
				txtIndexSize.Text = "";
				if (StringType.StrCmp(cmbCollType.Text, "Residential", false) == 0)
				{
					FileSystem.FileCopy(Application.StartupPath + "/Templates/HomeCollectionTemplate.dat", Application.StartupPath + "/COLLFile.dat");
				}
				else if (StringType.StrCmp(cmbCollType.Text, "Community", false) == 0)
				{
					FileSystem.FileCopy(Application.StartupPath + "/Templates/CommCollectionTemplate.dat", Application.StartupPath + "/COLLFile.dat");
				}
				else
				{
					FileSystem.FileCopy(Application.StartupPath + "/Templates/BothCollectionTemplate.dat", Application.StartupPath + "/COLLFile.dat");
				}
				lstDatFileNames.Items.Add((object)"COLLFile.dat");
				string text2 = txtCollID.Text;
				Reverse(text2, 1L);
				lstGroups.Items.Add((object)"FFFFFFFF");
				lstInstance2.Items.Add((object)"00000000");
				long num2 = FileSystem.FileLen(Application.StartupPath + "/COLLFile.dat");
				string filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/COLLFile.dat"));
				Fill(filler, 1L);
				long num3 = 96L;
				string filler2 = Conversion.Hex(num3);
				Fill(filler2, 2L);
				num3 += num2;
				lstResources.Items.Add((object)"9D354F6C");
				FileSystem.FileCopy(txtImgPath.Text, Application.StartupPath + "/CollIcon.dat");
				lstDatFileNames.Items.Add((object)"CollIcon.dat");
				text2 = "00000001";
				Reverse(text2, 1L);
				lstGroups.Items.Add((object)"FFFFFFFF");
				lstInstance2.Items.Add((object)"00000000");
				num2 = FileSystem.FileLen(Application.StartupPath + "/CollIcon.dat");
				filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/CollIcon.dat"));
				Fill(filler, 1L);
				filler2 = Conversion.Hex(num3);
				Fill(filler2, 2L);
				num3 += num2;
				lstResources.Items.Add((object)"ACDB6D85");
				FileSystem.FileCopy(Application.StartupPath + "/Templates/STRTemplate.dat", Application.StartupPath + "/STRFile.dat");
				lstDatFileNames.Items.Add((object)"STRFile.dat");
				text2 = "00000001";
				Reverse(text2, 1L);
				lstGroups.Items.Add((object)"FFFFFFFF");
				lstInstance2.Items.Add((object)"00000000");
				StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "/Temp.txt");
				streamWriter.Write(txtCollName.Text);
				streamWriter.Close();
				FileStream fileStream = new FileStream(Application.StartupPath + "/Temp.txt", FileMode.Open, FileAccess.ReadWrite);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				long num4 = 1L;
				long num5 = FileSystem.FileLen(Application.StartupPath + "/Temp.txt");
				FileStream fileStream2 = new FileStream(Application.StartupPath + "/STRFile.dat", FileMode.Append, FileAccess.Write);
				BinaryWriter binaryWriter = new BinaryWriter(fileStream2);
				long num6 = FileSystem.FileLen(Application.StartupPath + "/STRFile.dat");
				byte[] buffer = binaryReader.ReadBytes((int)num5);
				binaryWriter.Write(buffer);
				byte value = 0;
				binaryWriter.Write(value);
				binaryWriter.Write(value);
				fileStream2.Close();
				binaryWriter.Close();
				fileStream.Close();
				binaryReader.Close();
				num2 = FileSystem.FileLen(Application.StartupPath + "/STRFile.dat");
				filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/STRFile.dat"));
				Fill(filler, 1L);
				filler2 = Conversion.Hex(num3);
				Fill(filler2, 2L);
				num3 += num2;
				lstResources.Items.Add((object)"23525453");
				lstDatFileNames.Items.Add((object)"3IDRStrImg.dat");
				FileStream fileStream3 = new FileStream(Application.StartupPath + "/3IDRStrImg.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
				BinaryWriter binaryWriter2 = new BinaryWriter(fileStream3);
				value = (byte)CCB.Math.ToDec("EF");
				binaryWriter2.Write(value);
				value = (byte)CCB.Math.ToDec("BE");
				binaryWriter2.Write(value);
				value = (byte)CCB.Math.ToDec("AD");
				binaryWriter2.Write(value);
				value = (byte)CCB.Math.ToDec("DE");
				binaryWriter2.Write(value);
				value = (byte)CCB.Math.ToDec("02");
				binaryWriter2.Write(value);
				value = (byte)CCB.Math.ToDec("00");
				binaryWriter2.Write(value);
				binaryWriter2.Write(value);
				binaryWriter2.Write(value);
				value = (byte)CCB.Math.ToDec("02");
				binaryWriter2.Write(value);
				value = (byte)CCB.Math.ToDec("00");
				binaryWriter2.Write(value);
				binaryWriter2.Write(value);
				binaryWriter2.Write(value);
				binaryWriter2.Close();
				fileStream3.Close();
				Reverse("856DDBAC", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("FFFFFFFF", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("00000001", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("00000000", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("53545223", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("FFFFFFFF", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("00000001", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("00000000", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				text2 = txtCollID.Text;
				Reverse(text2, 1L);
				lstGroups.Items.Add((object)"FFFFFFFF");
				lstInstance2.Items.Add((object)"00000000");
				num2 = FileSystem.FileLen(Application.StartupPath + "/3IDRStrImg.dat");
				filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/3IDRStrImg.dat"));
				Fill(filler, 1L);
				filler2 = Conversion.Hex(num3);
				Fill(filler2, 2L);
				num3 += num2;
				lstResources.Items.Add((object)"646750AC");
				long num7 = lstListOfItems.Items.Count;
				for (num4 = 0L; num4 != num7; num4++)
				{
					string dataString = StringType.FromObject(lstListOfItems.Items[(int)num4]);
					text2 = txtCollID.Text;
					Make3IDR(dataString, num4, text2);
					lstDatFileNames.Items.Add((object)("3IDRNumber" + StringType.FromLong(num4) + ".dat"));
					num5 = CCB.Math.ToDec(text2);
					num5 = num5 + num4 + 1L;
					text2 = Conversion.Hex(num5);
					Reverse(text2, 1L);
					lstGroups.Items.Add((object)"FFFFFFFF");
					lstInstance2.Items.Add((object)"00000000");
					lstResources.Items.Add((object)"646750AC");
					num2 = FileSystem.FileLen(Application.StartupPath + "/3IDRNumber" + StringType.FromLong(num4) + ".dat");
					filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/3IDRNumber" + StringType.FromLong(num4) + ".dat"));
					Fill(filler, 1L);
					filler2 = Conversion.Hex(num3);
					Fill(filler2, 2L);
					num3 += num2;
					MakeBINX(num4);
					text2 = txtCollID.Text;
					num5 = CCB.Math.ToDec(text2);
					num5 = num5 + num4 + 1L;
					text2 = Conversion.Hex(num5);
					Reverse(text2, 1L);
					lstGroups.Items.Add((object)"FFFFFFFF");
					lstInstance2.Items.Add((object)"00000000");
					lstResources.Items.Add((object)"390F560C");
					num2 = FileSystem.FileLen(Application.StartupPath + "/BINXResource" + StringType.FromLong(num4) + ".dat");
					filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/BINXResource" + StringType.FromLong(num4) + ".dat"));
					Fill(filler, 1L);
					filler2 = Conversion.Hex(num3);
					Fill(filler2, 2L);
					num3 += num2;
				}
				MakeHeader(StringType.FromLong(num3), StringType.FromInteger(lstDatFileNames.Items.Count), StringType.FromInteger(lstDatFileNames.Items.Count * 20));
				MakeIndex();
				CombineDats();
				if (chkCompression.Checked)
				{
					startProcess(Application.StartupPath + "/dbpf-recompress.exe", txtPackagePath.Text);
				}
				ChangeSta("File successfully saved!");
				OldStaText = "File successfully saved!";
			}
		}
	}

	public object Reverse(string Convertee, long Action, string PackagePath = "")
	{
		string text = Convertee;
		text = Strings.Right(text, 2);
		string text2 = text;
		text = Convertee;
		text = Strings.Right(text, 4);
		text = Strings.Left(text, 2);
		text2 += text;
		text = Convertee;
		text = Strings.Left(text, 4);
		text = Strings.Right(text, 2);
		text2 += text;
		text = Convertee;
		text = Strings.Left(text, 2);
		text2 += text;
		checked
		{
			switch (Action)
			{
			case 1L:
				lstInstance.Items.Add((object)text2);
				break;
			case 2L:
				lstSize.Items.Add((object)text2);
				break;
			case 3L:
				lstOffset.Items.Add((object)text2);
				break;
			case 4L:
			{
				FileStream fileStream = new FileStream(PackagePath, FileMode.Append, FileAccess.Write);
				BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				text = Convertee;
				text = Strings.Right(text, 2);
				byte value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = Convertee;
				text = Strings.Right(text, 4);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = Convertee;
				text = Strings.Left(text, 4);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = Convertee;
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				binaryWriter.Close();
				fileStream.Close();
				break;
			}
			}
			object result = default(object);
			return result;
		}
	}

	public object Fill(string Filler, long Action)
	{
		if (Strings.Len(Filler) == 1)
		{
			Filler = "0000000" + Filler;
		}
		else if (Strings.Len(Filler) == 2)
		{
			Filler = "000000" + Filler;
		}
		else if (Strings.Len(Filler) == 3)
		{
			Filler = "00000" + Filler;
		}
		else if (Strings.Len(Filler) == 4)
		{
			Filler = "0000" + Filler;
		}
		else if (Strings.Len(Filler) == 5)
		{
			Filler = "000" + Filler;
		}
		else if (Strings.Len(Filler) == 6)
		{
			Filler = "00" + Filler;
		}
		else if (Strings.Len(Filler) == 7)
		{
			Filler = "0" + Filler;
		}
		switch (Action)
		{
		case 1L:
			Reverse(Filler, 2L);
			break;
		case 2L:
			Reverse(Filler, 3L);
			break;
		}
		object result = default(object);
		return result;
	}

	public object PullThumbnail(string PackagePath, int Mode = 1)
	{
		checked
		{
			PackagePath = ((PackagePath.LastIndexOf("/") <= 0) ? PackagePath.Remove(0, PackagePath.LastIndexOf("\\") + 1) : PackagePath.Remove(0, PackagePath.LastIndexOf("/") + 1));
			PackagePath = PackagePath.Remove(PackagePath.Length - 8, 8);
			PackagePath = Strings.LCase(PackagePath);
			if (ThumbPackageLoaded != 1)
			{
				if (File.Exists(txtThumbDir.Text + "/ObjectThumbnails.package"))
				{
					FindResource(txtThumbDir.Text + "/ObjectThumbnails.package");
					ThumbPackageLoaded = 1;
				}
				else
				{
					ChangeSta("Thumbnail Package Not Found! Previews will not be shown.");
					ThumbPackageLoaded = 0;
				}
			}
			HandleThumbnail(txtThumbDir.Text + "/ObjectThumbnails.package", "7F" + CCB.Math.ToHex(Hashes.GetCrc24(PackagePath).ToString()), Mode);
			object result = default(object);
			return result;
		}
	}

	public object Make3IDR(string DataString, long Count, string Instance)
	{
		FileStream fileStream = new FileStream(Application.StartupPath + "/3IDRNumber" + StringType.FromLong(Count) + ".dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		string text = Strings.Right(DataString, 41);
		text = Strings.Left(text, 8);
		string text2 = Strings.Right(DataString, 41);
		text2 = Strings.Left(text2, 19);
		text2 = Strings.Right(text2, 8);
		string text3 = Strings.Right(DataString, 41);
		text3 = Strings.Right(text3, 19);
		text3 = Strings.Left(text3, 8);
		string convertee = Strings.Right(DataString, 8);
		string packagePath = Application.StartupPath + "/3IDRNumber" + StringType.FromLong(Count) + ".dat";
		checked
		{
			byte value = (byte)CCB.Math.ToDec("EF");
			binaryWriter.Write(value);
			value = (byte)CCB.Math.ToDec("BE");
			binaryWriter.Write(value);
			value = (byte)CCB.Math.ToDec("AD");
			binaryWriter.Write(value);
			value = (byte)CCB.Math.ToDec("DE");
			binaryWriter.Write(value);
			value = (byte)CCB.Math.ToDec("02");
			binaryWriter.Write(value);
			value = (byte)CCB.Math.ToDec("00");
			binaryWriter.Write(value);
			binaryWriter.Write(value);
			binaryWriter.Write(value);
			value = (byte)CCB.Math.ToDec("03");
			binaryWriter.Write(value);
			value = (byte)CCB.Math.ToDec("00");
			binaryWriter.Write(value);
			binaryWriter.Write(value);
			binaryWriter.Write(value);
			fileStream.Close();
			binaryWriter.Close();
			Reverse("00000000", 4L, packagePath);
			Reverse("00000000", 4L, packagePath);
			Reverse("00000000", 4L, packagePath);
			Reverse("00000000", 4L, packagePath);
			Reverse("6C4F359D", 4L, packagePath);
			Reverse("FFFFFFFF", 4L, packagePath);
			Reverse(Instance, 4L, packagePath);
			Reverse("00000000", 4L, packagePath);
			Reverse(text, 4L, packagePath);
			Reverse(text3, 4L, packagePath);
			Reverse(convertee, 4L, packagePath);
			Reverse(text2, 4L, packagePath);
			object result = default(object);
			return result;
		}
	}

	public object MakeBINX(long Number)
	{
		FileSystem.FileCopy(Application.StartupPath + "/Templates/BINXTemplate.dat", Application.StartupPath + "/BINXResource" + StringType.FromLong(Number) + ".dat");
		lstDatFileNames.Items.Add((object)("BINXResource" + StringType.FromLong(Number) + ".dat"));
		string text = Conversion.Hex(Number);
		if (Strings.Len(text) == 1)
		{
			text = "0000000" + text;
		}
		else if (Strings.Len(text) == 2)
		{
			text = "000000" + text;
		}
		else if (Strings.Len(text) == 3)
		{
			text = "00000" + text;
		}
		else if (Strings.Len(text) == 4)
		{
			text = "0000" + text;
		}
		else if (Strings.Len(text) == 5)
		{
			text = "000" + text;
		}
		else if (Strings.Len(text) == 6)
		{
			text = "00" + text;
		}
		else if (Strings.Len(text) == 7)
		{
			text = "0" + text;
		}
		FileStream fileStream = new FileStream(Application.StartupPath + "/BINXResource" + StringType.FromLong(Number) + ".dat", FileMode.Open, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		string text2 = text;
		text2 = Strings.Right(text2, 2);
		checked
		{
			byte value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.BaseStream.Position = 166L;
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Right(text2, 4);
			text2 = Strings.Left(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Left(text2, 4);
			text2 = Strings.Right(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Left(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			binaryWriter.Close();
			fileStream.Close();
			object result = default(object);
			return result;
		}
	}

	public object MakeHeader(string Offset, string Number, string Size)
	{
		FileSystem.FileCopy(Application.StartupPath + "/Templates/Header.dat", Application.StartupPath + "/Header.dat");
		FileStream fileStream = new FileStream(Application.StartupPath + "/Header.dat", FileMode.Open, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		string text = Conversion.Hex((object)Offset);
		if (Strings.Len(text) == 1)
		{
			text = "0000000" + text;
		}
		else if (Strings.Len(text) == 2)
		{
			text = "000000" + text;
		}
		else if (Strings.Len(text) == 3)
		{
			text = "00000" + text;
		}
		else if (Strings.Len(text) == 4)
		{
			text = "0000" + text;
		}
		else if (Strings.Len(text) == 5)
		{
			text = "000" + text;
		}
		else if (Strings.Len(text) == 6)
		{
			text = "00" + text;
		}
		else if (Strings.Len(text) == 7)
		{
			text = "0" + text;
		}
		string text2 = text;
		text2 = Strings.Right(text2, 2);
		checked
		{
			byte value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.BaseStream.Position = 40L;
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Right(text2, 4);
			text2 = Strings.Left(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Left(text2, 4);
			text2 = Strings.Right(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Left(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text = Conversion.Hex((object)Number);
			if (Strings.Len(text) == 1)
			{
				text = "0000000" + text;
			}
			else if (Strings.Len(text) == 2)
			{
				text = "000000" + text;
			}
			else if (Strings.Len(text) == 3)
			{
				text = "00000" + text;
			}
			else if (Strings.Len(text) == 4)
			{
				text = "0000" + text;
			}
			else if (Strings.Len(text) == 5)
			{
				text = "000" + text;
			}
			else if (Strings.Len(text) == 6)
			{
				text = "00" + text;
			}
			else if (Strings.Len(text) == 7)
			{
				text = "0" + text;
			}
			text2 = text;
			text2 = Strings.Right(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.BaseStream.Position = 36L;
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Right(text2, 4);
			text2 = Strings.Left(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Left(text2, 4);
			text2 = Strings.Right(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Left(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text = Conversion.Hex((object)Size);
			if (Strings.Len(text) == 1)
			{
				text = "0000000" + text;
			}
			else if (Strings.Len(text) == 2)
			{
				text = "000000" + text;
			}
			else if (Strings.Len(text) == 3)
			{
				text = "00000" + text;
			}
			else if (Strings.Len(text) == 4)
			{
				text = "0000" + text;
			}
			else if (Strings.Len(text) == 5)
			{
				text = "000" + text;
			}
			else if (Strings.Len(text) == 6)
			{
				text = "00" + text;
			}
			else if (Strings.Len(text) == 7)
			{
				text = "0" + text;
			}
			text2 = text;
			text2 = Strings.Right(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.BaseStream.Position = 44L;
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Right(text2, 4);
			text2 = Strings.Left(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Left(text2, 4);
			text2 = Strings.Right(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			text2 = text;
			text2 = Strings.Left(text2, 2);
			value = (byte)CCB.Math.ToDec(text2);
			binaryWriter.Write(value);
			binaryWriter.Close();
			fileStream.Close();
			object result = default(object);
			return result;
		}
	}

	public object MakeIndex()
	{
		FileStream fileStream = new FileStream(Application.StartupPath + "/Index.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		long num = lstResources.Items.Count;
		long num2 = 0L;
		long num3 = 1L;
		checked
		{
			for (; num2 != num; num2++)
			{
				string text = StringType.FromObject(lstResources.Items[(int)num2]);
				text = Strings.Left(text, 2);
				byte value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstResources.Items[(int)num2]);
				text = Strings.Left(text, 4);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstResources.Items[(int)num2]);
				text = Strings.Right(text, 4);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstResources.Items[(int)num2]);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstGroups.Items[(int)num2]);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstGroups.Items[(int)num2]);
				text = Strings.Left(text, 4);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstGroups.Items[(int)num2]);
				text = Strings.Right(text, 4);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstGroups.Items[(int)num2]);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstInstance.Items[(int)num2]);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstInstance.Items[(int)num2]);
				text = Strings.Left(text, 4);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstInstance.Items[(int)num2]);
				text = Strings.Right(text, 4);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstInstance.Items[(int)num2]);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstOffset.Items[(int)num2]);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstOffset.Items[(int)num2]);
				text = Strings.Left(text, 4);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstOffset.Items[(int)num2]);
				text = Strings.Right(text, 4);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstOffset.Items[(int)num2]);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstSize.Items[(int)num2]);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstSize.Items[(int)num2]);
				text = Strings.Left(text, 4);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstSize.Items[(int)num2]);
				text = Strings.Right(text, 4);
				text = Strings.Left(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
				text = StringType.FromObject(lstSize.Items[(int)num2]);
				text = Strings.Right(text, 2);
				value = (byte)CCB.Math.ToDec(text);
				binaryWriter.Write(value);
			}
			binaryWriter.Close();
			fileStream.Close();
			object result = default(object);
			return result;
		}
	}

	public object CombineDats()
	{
		checked
		{
			IO.AppendDats(txtPackagePath.Text, Application.StartupPath + "/Header.dat", (int)FileSystem.FileLen(Application.StartupPath + "/Header.dat"));
			long num = lstDatFileNames.Items.Count;
			string text;
			long num3;
			for (long num2 = 0L; num2 != num; num2++)
			{
				text = StringType.FromObject(ObjectType.AddObj((object)(Application.StartupPath + "/"), lstDatFileNames.Items[(int)num2]));
				num3 = FileSystem.FileLen(text);
				IO.AppendDats(txtPackagePath.Text, text, (int)num3);
			}
			text = Application.StartupPath + "/Index.dat";
			num3 = FileSystem.FileLen(text);
			IO.AppendDats(txtPackagePath.Text, text, (int)num3);
			object result = default(object);
			return result;
		}
	}

	private void cmdOptions_Click(object sender, EventArgs e)
	{
		HideUI(1L);
	}

	private void cmdCloseOptions_Click(object sender, EventArgs e)
	{
		ShowUI(1L);
		StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "/Options.txt");
		streamWriter.WriteLine(txtCollDir.Text);
		int value = (chkWarningOff.Checked ? 1 : 0);
		streamWriter.WriteLine(value);
		int value2 = (chkCompression.Checked ? 1 : 0);
		streamWriter.WriteLine(value2);
		streamWriter.WriteLine(txtThumbDir.Text);
		streamWriter.Close();
	}

	private void cmdFindCollDir_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((CommonDialog)FolderBrowserDialog1).ShowDialog();
		if (StringType.StrCmp(FolderBrowserDialog1.SelectedPath, "", false) != 0)
		{
			txtCollDir.Text = FolderBrowserDialog1.SelectedPath;
		}
	}

	private void cmdFindThumbDir_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((CommonDialog)FolderBrowserDialog1).ShowDialog();
		if (StringType.StrCmp(FolderBrowserDialog1.SelectedPath, "", false) != 0)
		{
			txtThumbDir.Text = FolderBrowserDialog1.SelectedPath;
		}
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		int try0000_dispatch = -1;
		int num = default(int);
		int num2 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
				{
					IOTools.killFiles(Application.StartupPath + "/", "*.dat");
					IOTools.killFiles(Application.StartupPath + "/", "*.img");
					ProjectData.ClearProjectError();
					num = 1;
					StreamReader streamReader = new StreamReader(Application.StartupPath + "/Options.txt");
					string text = streamReader.ReadLine();
					txtCollDir.Text = text;
					text = streamReader.ReadLine();
					if (DoubleType.FromString(text) == 1.0)
					{
						chkWarningOff.Checked = true;
					}
					else
					{
						chkWarningOff.Checked = false;
					}
					text = streamReader.ReadLine();
					if (DoubleType.FromString(text) == 1.0)
					{
						chkCompression.Checked = true;
					}
					else
					{
						chkCompression.Checked = false;
					}
					text = streamReader.ReadLine();
					if (StringType.StrCmp(text, "PlaceHolderForThumbnailDir", false) == 0)
					{
						streamReader.Close();
						FileSystem.Kill(Application.StartupPath + "/Options.txt");
						txtCollDir.Text = "";
						chkWarningOff.Checked = false;
						chkCompression.Checked = false;
						break;
					}
					txtThumbDir.Text = text;
					streamReader.Close();
					ChangeSta("Configuration file loaded successfully!");
					OldStaText = "Configuration file loaded successfully!";
					goto end_IL_0000;
				}
				case 330:
					break;
				}
				ChangeSta("Configuration File Not Found; Please Set Options Now.");
				OldStaText = "Configuration File Not Found; Please Set Options Now.";
				HideUI(1L);
				end_IL_0000:;
			}
			catch (object obj) when ((obj is Exception && num != 0 && num2 == 0) ? true : false)
			{
				Exception obj2 = (Exception)obj;
				ProjectData.SetProjectError(obj2);
				Exception ex = obj2;
				if (num2 != 0)
				{
					break;
				}
				num2 = -1;
				switch (num)
				{
				case 1:
					try0000_dispatch = 330;
					break;
				default:
					throw;
				}
				continue;
			}
			break;
		}
		if (num2 != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private void cmdEditColl_Click(object sender, EventArgs e)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		txtPackagePath.Text = "";
		((FileDialog)cdlOpenFile).InitialDirectory = txtCollDir.Text;
		((FileDialog)cdlOpenFile).Title = "Open a Collection File to Edit";
		((FileDialog)cdlOpenFile).Filter = "Package Files (*.package)|*.package|All Files (*.*)|*.*";
		((FileDialog)cdlOpenFile).FileName = "";
		((CommonDialog)cdlOpenFile).ShowDialog();
		((FileDialog)cdlOpenFile).InitialDirectory = "";
		txtPackagePath.Text = ((FileDialog)cdlOpenFile).FileName;
		string text = Strings.LCase(txtPackagePath.Text);
		string text2 = ".package";
		long num = Strings.InStr(text, text2, (CompareMethod)1);
		if (num == 0L)
		{
			ChangeSta("File Not Loaded. Error: Doesn't appear to be a package file.");
			OldStaText = "File Not Loaded. Error: Doesn't appear to be a package file.";
		}
		else if (StringType.StrCmp(txtPackagePath.Text, "", false) != 0)
		{
			string text3 = txtPackagePath.Text;
			lstListOfItems.Items.Clear();
			txtCollName.Text = "";
			cmbCollType.Text = "";
			FindResource(text3);
			Handle3IDR(text3);
			HandleJPGImage(text3);
			HandleSTRFile(text3);
			HandleCOLLFile(text3);
			((Control)Command1).Enabled = true;
			((Control)cmdSaveColl).Enabled = true;
			((Control)cmdMoveUp).Enabled = true;
			((Control)cmdRemoveItem).Enabled = true;
			((Control)cmdMoveDown).Enabled = true;
			((Control)cmdBatchAdd).Enabled = true;
		}
	}

	public void Handle3IDR(string PackagePath)
	{
		string text = "AC506764";
		int count = lstResources.Items.Count;
		int num = 0;
		IOTools.killFiles(Application.StartupPath + "/", "*.dat");
		checked
		{
			while (num != count)
			{
				if (ObjectType.ObjTst((object)text, lstResources.Items[num], false) == 0)
				{
					long num2 = lstOffset.Items.Count;
					num2 = LongType.FromObject(lstOffset.Items[num]);
					long num3 = LongType.FromObject(lstSize.Items[num]);
					string text2 = StringType.FromObject(lstGroups.Items[num]);
					long num4 = num + 1;
					txtGroup.Text = text2;
					IO.DumpResourceData(PackagePath, (int)num2, (int)num4, (int)num3, text, Application.StartupPath);
					num++;
				}
				else
				{
					num++;
				}
			}
			lstDatFileNames.Items.Clear();
			string[] array = IOTools.GetsFilesNoDir(Application.StartupPath + "/", "*.dat");
			for (long num5 = 0L; num5 < array.Length; num5++)
			{
				lstDatFileNames.Items.Add((object)array[(int)num5]);
			}
			long num6 = lstDatFileNames.Items.Count;
			for (long num7 = 0L; num7 != num6; num7++)
			{
				string resourceName = StringType.FromObject(lstDatFileNames.Items[(int)num7]);
				lstDatFileNames.Items[(int)num7] = IO.Decompress(resourceName, (int)num7, Application.StartupPath);
			}
			for (long num7 = 0L; num7 != num6; num7++)
			{
				Pull3IDRInfo(StringType.FromObject(lstDatFileNames.Items[(int)num7]));
			}
		}
	}

	private void cmdAbout_Click(object sender, EventArgs e)
	{
		frmAbout frmAbout2 = new frmAbout();
		((Control)frmAbout2).Visible = true;
	}

	public void Pull3IDRInfo(string DatName)
	{
		int try0000_dispatch = -1;
		int num = default(int);
		FileStream fileStream = default(FileStream);
		BinaryReader binaryReader = default(BinaryReader);
		int num3 = default(int);
		while (true)
		{
			try
			{
				/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
				switch (try0000_dispatch)
				{
				default:
				{
					ProjectData.ClearProjectError();
					num = 1;
					fileStream = new FileStream(Application.StartupPath + "/" + DatName, FileMode.Open, FileAccess.ReadWrite);
					binaryReader = new BinaryReader(fileStream);
					long num2 = FileSystem.FileLen(Application.StartupPath + "/" + DatName);
					while (binaryReader.BaseStream.Position != num2)
					{
						byte b = binaryReader.ReadByte();
						byte b2 = binaryReader.ReadByte();
						byte b3 = binaryReader.ReadByte();
						byte b4 = binaryReader.ReadByte();
						string text = Conversion.Hex(b);
						string text2 = Conversion.Hex(b2);
						string text3 = Conversion.Hex(b3);
						string text4 = Conversion.Hex(b4);
						if (StringType.StrCmp(text, "0", false) == 0)
						{
							text = "00";
						}
						else if (Strings.Len(text) == 1)
						{
							text = "0" + text;
						}
						if (StringType.StrCmp(text2, "0", false) == 0)
						{
							text2 = "00";
						}
						else if (Strings.Len(text2) == 1)
						{
							text2 = "0" + text2;
						}
						if (StringType.StrCmp(text3, "0", false) == 0)
						{
							text3 = "00";
						}
						else if (Strings.Len(text3) == 1)
						{
							text3 = "0" + text3;
						}
						if (StringType.StrCmp(text4, "0", false) == 0)
						{
							text4 = "00";
						}
						else if (Strings.Len(text4) == 1)
						{
							text4 = "0" + text4;
						}
						string text5 = text + text2 + text3 + text4;
						if (StringType.StrCmp(text5, "9F3FDA69", false) == 0)
						{
							b = binaryReader.ReadByte();
							b2 = binaryReader.ReadByte();
							b3 = binaryReader.ReadByte();
							b4 = binaryReader.ReadByte();
							text = Conversion.Hex(b);
							text2 = Conversion.Hex(b2);
							text3 = Conversion.Hex(b3);
							text4 = Conversion.Hex(b4);
							if (StringType.StrCmp(text, "0", false) == 0)
							{
								text = "00";
							}
							else if (Strings.Len(text) == 1)
							{
								text = "0" + text;
							}
							if (StringType.StrCmp(text2, "0", false) == 0)
							{
								text2 = "00";
							}
							else if (Strings.Len(text2) == 1)
							{
								text2 = "0" + text2;
							}
							if (StringType.StrCmp(text3, "0", false) == 0)
							{
								text3 = "00";
							}
							else if (Strings.Len(text3) == 1)
							{
								text3 = "0" + text3;
							}
							if (StringType.StrCmp(text4, "0", false) == 0)
							{
								text4 = "00";
							}
							else if (Strings.Len(text4) == 1)
							{
								text4 = "0" + text4;
							}
							string text6 = text4 + text3 + text2 + text;
							b = binaryReader.ReadByte();
							b2 = binaryReader.ReadByte();
							b3 = binaryReader.ReadByte();
							b4 = binaryReader.ReadByte();
							text = Conversion.Hex(b);
							text2 = Conversion.Hex(b2);
							text3 = Conversion.Hex(b3);
							text4 = Conversion.Hex(b4);
							if (StringType.StrCmp(text, "0", false) == 0)
							{
								text = "00";
							}
							else if (Strings.Len(text) == 1)
							{
								text = "0" + text;
							}
							if (StringType.StrCmp(text2, "0", false) == 0)
							{
								text2 = "00";
							}
							else if (Strings.Len(text2) == 1)
							{
								text2 = "0" + text2;
							}
							if (StringType.StrCmp(text3, "0", false) == 0)
							{
								text3 = "00";
							}
							else if (Strings.Len(text3) == 1)
							{
								text3 = "0" + text3;
							}
							if (StringType.StrCmp(text4, "0", false) == 0)
							{
								text4 = "00";
							}
							else if (Strings.Len(text4) == 1)
							{
								text4 = "0" + text4;
							}
							string text7 = text4 + text3 + text2 + text;
							b = binaryReader.ReadByte();
							b2 = binaryReader.ReadByte();
							b3 = binaryReader.ReadByte();
							b4 = binaryReader.ReadByte();
							text = Conversion.Hex(b);
							text2 = Conversion.Hex(b2);
							text3 = Conversion.Hex(b3);
							text4 = Conversion.Hex(b4);
							if (StringType.StrCmp(text, "0", false) == 0)
							{
								text = "00";
							}
							else if (Strings.Len(text) == 1)
							{
								text = "0" + text;
							}
							if (StringType.StrCmp(text2, "0", false) == 0)
							{
								text2 = "00";
							}
							else if (Strings.Len(text2) == 1)
							{
								text2 = "0" + text2;
							}
							if (StringType.StrCmp(text3, "0", false) == 0)
							{
								text3 = "00";
							}
							else if (Strings.Len(text3) == 1)
							{
								text3 = "0" + text3;
							}
							if (StringType.StrCmp(text4, "0", false) == 0)
							{
								text4 = "00";
							}
							else if (Strings.Len(text4) == 1)
							{
								text4 = "0" + text4;
							}
							string text8 = text4 + text3 + text2 + text;
							string text9 = "69DA3F9F";
							string text10 = Common.Translate("0x" + text7, Application.StartupPath + "/MaxisObjectList.txt", 1L, 2L, ';');
							if (StringType.StrCmp(text10, "No Match Found", false) == 0)
							{
								text10 = Common.Translate("0x" + text7, Application.StartupPath + "/UserObjectList.txt", 1L, 2L, ';');
							}
							lstListOfItems.Items.Add((object)("Object: " + text10 + " - " + text9 + " - " + text8 + " - " + text6 + " - " + text7));
							break;
						}
						if (StringType.StrCmp(text5, "0E45DAE9", false) == 0)
						{
							b = binaryReader.ReadByte();
							b2 = binaryReader.ReadByte();
							b3 = binaryReader.ReadByte();
							b4 = binaryReader.ReadByte();
							text = Conversion.Hex(b);
							text2 = Conversion.Hex(b2);
							text3 = Conversion.Hex(b3);
							text4 = Conversion.Hex(b4);
							if (StringType.StrCmp(text, "0", false) == 0)
							{
								text = "00";
							}
							else if (Strings.Len(text) == 1)
							{
								text = "0" + text;
							}
							if (StringType.StrCmp(text2, "0", false) == 0)
							{
								text2 = "00";
							}
							else if (Strings.Len(text2) == 1)
							{
								text2 = "0" + text2;
							}
							if (StringType.StrCmp(text3, "0", false) == 0)
							{
								text3 = "00";
							}
							else if (Strings.Len(text3) == 1)
							{
								text3 = "0" + text3;
							}
							if (StringType.StrCmp(text4, "0", false) == 0)
							{
								text4 = "00";
							}
							else if (Strings.Len(text4) == 1)
							{
								text4 = "0" + text4;
							}
							string text6 = text4 + text3 + text2 + text;
							b = binaryReader.ReadByte();
							b2 = binaryReader.ReadByte();
							b3 = binaryReader.ReadByte();
							b4 = binaryReader.ReadByte();
							text = Conversion.Hex(b);
							text2 = Conversion.Hex(b2);
							text3 = Conversion.Hex(b3);
							text4 = Conversion.Hex(b4);
							if (StringType.StrCmp(text, "0", false) == 0)
							{
								text = "00";
							}
							else if (Strings.Len(text) == 1)
							{
								text = "0" + text;
							}
							if (StringType.StrCmp(text2, "0", false) == 0)
							{
								text2 = "00";
							}
							else if (Strings.Len(text2) == 1)
							{
								text2 = "0" + text2;
							}
							if (StringType.StrCmp(text3, "0", false) == 0)
							{
								text3 = "00";
							}
							else if (Strings.Len(text3) == 1)
							{
								text3 = "0" + text3;
							}
							if (StringType.StrCmp(text4, "0", false) == 0)
							{
								text4 = "00";
							}
							else if (Strings.Len(text4) == 1)
							{
								text4 = "0" + text4;
							}
							string text7 = text4 + text3 + text2 + text;
							b = binaryReader.ReadByte();
							b2 = binaryReader.ReadByte();
							b3 = binaryReader.ReadByte();
							b4 = binaryReader.ReadByte();
							text = Conversion.Hex(b);
							text2 = Conversion.Hex(b2);
							text3 = Conversion.Hex(b3);
							text4 = Conversion.Hex(b4);
							if (StringType.StrCmp(text, "0", false) == 0)
							{
								text = "00";
							}
							else if (Strings.Len(text) == 1)
							{
								text = "0" + text;
							}
							if (StringType.StrCmp(text2, "0", false) == 0)
							{
								text2 = "00";
							}
							else if (Strings.Len(text2) == 1)
							{
								text2 = "0" + text2;
							}
							if (StringType.StrCmp(text3, "0", false) == 0)
							{
								text3 = "00";
							}
							else if (Strings.Len(text3) == 1)
							{
								text3 = "0" + text3;
							}
							if (StringType.StrCmp(text4, "0", false) == 0)
							{
								text4 = "00";
							}
							else if (Strings.Len(text4) == 1)
							{
								text4 = "0" + text4;
							}
							string text8 = text4 + text3 + text2 + text;
							string text9 = "E9DA450E";
							if (StringType.StrCmp(text6, "00000001", false) == 0)
							{
								lstListOfItems.Items.Add((object)("Floor - " + text9 + " - " + text8 + " - " + text6 + " - " + text7));
							}
							else
							{
								lstListOfItems.Items.Add((object)("Wall - " + text9 + " - " + text8 + " - " + text6 + " - " + text7));
							}
							break;
						}
					}
					binaryReader.Close();
					fileStream.Close();
					break;
				}
				case 2557:
					break;
				}
				binaryReader.Close();
				fileStream.Close();
			}
			catch (object obj) when ((obj is Exception && num != 0 && num3 == 0) ? true : false)
			{
				Exception obj2 = (Exception)obj;
				ProjectData.SetProjectError(obj2);
				Exception ex = obj2;
				if (num3 != 0)
				{
					break;
				}
				num3 = -1;
				switch (num)
				{
				case 1:
					try0000_dispatch = 2557;
					break;
				default:
					throw;
				}
				continue;
			}
			break;
		}
		if (num3 != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	public void HandleJPGImage(string PackagePath, long Action = -1L)
	{
		int try0000_dispatch = -1;
		checked
		{
			int num8 = default(int);
			int num10 = default(int);
			while (true)
			{
				try
				{
					/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
					switch (try0000_dispatch)
					{
					default:
					{
						string text = "856DDBAC";
						int count = lstResources.Items.Count;
						int num = 0;
						IOTools.killFiles(Application.StartupPath + "/", "*.dat");
						while (num != count)
						{
							if (ObjectType.ObjTst((object)text, lstResources.Items[num], false) == 0)
							{
								long num2 = lstOffset.Items.Count;
								num2 = LongType.FromObject(lstOffset.Items[num]);
								long num3 = LongType.FromObject(lstSize.Items[num]);
								string text2 = StringType.FromObject(lstGroups.Items[num]);
								long num4 = num + 1;
								txtGroup.Text = text2;
								IO.DumpResourceData(PackagePath, (int)num2, (int)num4, (int)num3, text, Application.StartupPath);
								num++;
							}
							else
							{
								num++;
							}
						}
						lstDatFileNames.Items.Clear();
						string[] array = IOTools.GetsFilesNoDir(Application.StartupPath + "/", "*.dat");
						for (long num5 = 0L; num5 < array.Length; num5++)
						{
							lstDatFileNames.Items.Add((object)array[(int)num5]);
						}
						long num6 = lstDatFileNames.Items.Count;
						for (long num7 = 0L; num7 != num6; num7++)
						{
							string resourceName = StringType.FromObject(lstDatFileNames.Items[(int)num7]);
							lstDatFileNames.Items[(int)num7] = IO.Decompress(resourceName, (int)num7, Application.StartupPath);
						}
						for (long num7 = 0L; num7 != num6; num7++)
						{
							ProjectData.ClearProjectError();
							num8 = 1;
							Random random = new Random();
							long num9 = random.Next(1, 65535);
							FileSystem.Rename(StringType.FromObject(ObjectType.StrCatObj((object)(Application.StartupPath + "/"), lstDatFileNames.Items[(int)num7])), Application.StartupPath + "/" + StringType.FromLong(num9) + ".img");
							txtImgPath.Text = Application.StartupPath + "/" + StringType.FromLong(num9) + ".img";
							if (Action != 2L)
							{
								Picture1.Image = Image.FromFile(Application.StartupPath + "/" + StringType.FromLong(num9) + ".img");
								ChangeSta("Collection and image loaded successfully!");
								OldStaText = "Collection and image loaded successfully!";
							}
						}
						break;
					}
					case 704:
						ChangeSta("Collection loaded successfully, but image is not able to be displayed. Will be resaved intact though.");
						OldStaText = "Collection loaded successfully, but image is not able to be displayed. Will be resaved intact though.";
						break;
					}
				}
				catch (object obj) when ((obj is Exception && num8 != 0 && num10 == 0) ? true : false)
				{
					Exception obj2 = (Exception)obj;
					ProjectData.SetProjectError(obj2);
					Exception ex = obj2;
					if (num10 != 0)
					{
						break;
					}
					num10 = -1;
					switch (num8)
					{
					case 1:
						try0000_dispatch = 704;
						break;
					default:
						throw;
					}
					continue;
				}
				break;
			}
			if (num10 != 0)
			{
				ProjectData.ClearProjectError();
			}
		}
	}

	public void HandleThumbnail(string PackagePath, string Instance2ID, int Mode = 1)
	{
		int try0000_dispatch = -1;
		checked
		{
			int num7 = default(int);
			int num9 = default(int);
			while (true)
			{
				try
				{
					/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
					switch (try0000_dispatch)
					{
					default:
					{
						string resourceType = "C15029AC";
						int count = lstResources.Items.Count;
						int i = 0;
						IOTools.killFiles(Application.StartupPath + "/", "*.dat");
						Instance2ID = Strings.UCase(Instance2ID);
						for (; i != count; i++)
						{
							if (ObjectType.ObjTst((object)Instance2ID, lstInstance2.Items[i], false) == 0)
							{
								long num = lstOffset.Items.Count;
								num = LongType.FromObject(lstOffset.Items[i]);
								long num2 = LongType.FromObject(lstSize.Items[i]);
								string text = StringType.FromObject(lstGroups.Items[i]);
								long num3 = i + 1;
								txtGroup.Text = text;
								IO.DumpResourceData(PackagePath, (int)num, (int)num3, (int)num2, resourceType, Application.StartupPath);
								break;
							}
						}
						lstDatFileNames.Items.Clear();
						string[] array = IOTools.GetsFilesNoDir(Application.StartupPath + "/", "*.dat");
						long num4 = 0L;
						if (array.Length == 0)
						{
							if (Mode == 2)
							{
								PictureBox2.Image = Image.FromFile(Application.StartupPath + "/PreviewNotFound.jpg");
							}
							else
							{
								PictureBox1.Image = Image.FromFile(Application.StartupPath + "/PreviewNotFound.jpg");
							}
							break;
						}
						for (; num4 < array.Length; num4++)
						{
							lstDatFileNames.Items.Add((object)array[(int)num4]);
						}
						long num5 = lstDatFileNames.Items.Count;
						for (long num6 = 0L; num6 != num5; num6++)
						{
							string resourceName = StringType.FromObject(lstDatFileNames.Items[(int)num6]);
							lstDatFileNames.Items[(int)num6] = IO.Decompress(resourceName, (int)num6, Application.StartupPath);
						}
						for (long num6 = 0L; num6 != num5; num6++)
						{
							ProjectData.ClearProjectError();
							num7 = 1;
							Random random = new Random();
							long num8 = random.Next(1, 65535);
							FileSystem.Rename(StringType.FromObject(ObjectType.StrCatObj((object)(Application.StartupPath + "/"), lstDatFileNames.Items[(int)num6])), Application.StartupPath + "/" + StringType.FromLong(num8) + ".img");
							txtImgPath.Text = Application.StartupPath + "/" + StringType.FromLong(num8) + ".img";
							if (Mode == 2)
							{
								PictureBox2.Image = Image.FromFile(Application.StartupPath + "/" + StringType.FromLong(num8) + ".img");
							}
							else
							{
								PictureBox1.Image = Image.FromFile(Application.StartupPath + "/" + StringType.FromLong(num8) + ".img");
							}
							ChangeSta("Object and preview image loaded successfully!");
							OldStaText = "Object and preview image loaded successfully!";
						}
						break;
					}
					case 827:
						ChangeSta("Object loaded successfully, but image unable to be found.");
						OldStaText = "Object loaded successfully, but image unable to be found.";
						break;
					}
				}
				catch (object obj) when ((obj is Exception && num7 != 0 && num9 == 0) ? true : false)
				{
					Exception obj2 = (Exception)obj;
					ProjectData.SetProjectError(obj2);
					Exception ex = obj2;
					if (num9 != 0)
					{
						break;
					}
					num9 = -1;
					switch (num7)
					{
					case 1:
						try0000_dispatch = 827;
						break;
					default:
						throw;
					}
					continue;
				}
				break;
			}
			if (num9 != 0)
			{
				ProjectData.ClearProjectError();
			}
		}
	}

	public void HandleCOLLFile(string PackagePath)
	{
		string text = "6C4F359D";
		int count = lstResources.Items.Count;
		int num = 0;
		IOTools.killFiles(Application.StartupPath + "/", "*.dat");
		checked
		{
			while (num != count)
			{
				if (ObjectType.ObjTst((object)text, lstResources.Items[num], false) == 0)
				{
					long num2 = lstOffset.Items.Count;
					num2 = LongType.FromObject(lstOffset.Items[num]);
					long num3 = LongType.FromObject(lstSize.Items[num]);
					string text2 = StringType.FromObject(lstGroups.Items[num]);
					string text3 = StringType.FromObject(lstInstance.Items[num]);
					txtCollID.Text = text3;
					long num4 = num + 1;
					txtGroup.Text = text2;
					IO.DumpResourceData(PackagePath, (int)num2, (int)num4, (int)num3, text, Application.StartupPath);
					num++;
				}
				else
				{
					num++;
				}
			}
			lstDatFileNames.Items.Clear();
			string[] array = IOTools.GetsFilesNoDir(Application.StartupPath + "/", "*.dat");
			for (long num5 = 0L; num5 < array.Length; num5++)
			{
				lstDatFileNames.Items.Add((object)array[(int)num5]);
			}
			long num6 = lstDatFileNames.Items.Count;
			for (long num7 = 0L; num7 != num6; num7++)
			{
				string resourceName = StringType.FromObject(lstDatFileNames.Items[(int)num7]);
				lstDatFileNames.Items[(int)num7] = IO.Decompress(resourceName, (int)num7, Application.StartupPath);
			}
			for (long num7 = 0L; num7 != num6; num7++)
			{
				switch (FileSystem.FileLen(StringType.FromObject(ObjectType.AddObj((object)(Application.StartupPath + "/"), lstDatFileNames.Items[(int)num7]))))
				{
				case 197L:
					cmbCollType.Text = "Residential";
					break;
				case 209L:
					cmbCollType.Text = "Community";
					break;
				case 200L:
					cmbCollType.Text = "Both (PETS ONLY)";
					break;
				case 845L:
					cmbCollType.Text = "Residential";
					break;
				default:
					cmbCollType.Text = "";
					break;
				}
			}
		}
	}

	public void HandleSTRFile(string PackagePath)
	{
		string text = "53545223";
		int count = lstResources.Items.Count;
		int num = 0;
		IOTools.killFiles(Application.StartupPath + "/", "*.dat");
		checked
		{
			while (num != count)
			{
				if (ObjectType.ObjTst((object)text, lstResources.Items[num], false) == 0)
				{
					long num2 = lstOffset.Items.Count;
					num2 = LongType.FromObject(lstOffset.Items[num]);
					long num3 = LongType.FromObject(lstSize.Items[num]);
					string text2 = StringType.FromObject(lstGroups.Items[num]);
					string text3 = StringType.FromObject(lstInstance.Items[num]);
					txtCollID.Text = text3;
					long num4 = num + 1;
					txtGroup.Text = text2;
					IO.DumpResourceData(PackagePath, (int)num2, (int)num4, (int)num3, text, Application.StartupPath);
					num++;
				}
				else
				{
					num++;
				}
			}
			lstDatFileNames.Items.Clear();
			string[] array = IOTools.GetsFilesNoDir(Application.StartupPath + "/", "*.dat");
			for (long num5 = 0L; num5 < array.Length; num5++)
			{
				lstDatFileNames.Items.Add((object)array[(int)num5]);
			}
			long num6 = lstDatFileNames.Items.Count;
			for (long num7 = 0L; num7 != num6; num7++)
			{
				string resourceName = StringType.FromObject(lstDatFileNames.Items[(int)num7]);
				lstDatFileNames.Items[(int)num7] = IO.Decompress(resourceName, (int)num7, Application.StartupPath);
			}
			for (long num7 = 0L; num7 != num6; num7++)
			{
				DumpDesc(StringType.FromObject(lstDatFileNames.Items[(int)num7]), 2L);
			}
		}
	}

	private void cmdBatchAdd_Click(object sender, EventArgs e)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		lstBatchAdd.Items.Clear();
		lstBatchFileList.Items.Clear();
		lstBatchCategories.Items.Clear();
		((Control)lblBatchAddTotal).Text = "Total Items: 0";
		FolderBrowserDialog1.Description = "Choose a Package Directory";
		FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
		((CommonDialog)FolderBrowserDialog1).ShowDialog();
		txtPackagePath.Text = FolderBrowserDialog1.SelectedPath;
		lstDatFileNames.Items.Clear();
		checked
		{
			if (StringType.StrCmp(txtPackagePath.Text, "", false) != 0)
			{
				HideUI(2L);
				HideUI(3L);
				Recursive(FolderBrowserDialog1.SelectedPath);
				long num = lstRecursive.Items.Count;
				long num2 = 0L;
				long num3 = lstRecursive.Items.Count;
				ProgressBar1.Maximum = (int)(num3 + 1L);
				ProgressBar1.Value = 0;
				ProgressBar1.Minimum = 0;
				((Control)lblProgressDesc).Text = "Sorting Through Custom Content...";
				Application.DoEvents();
				while (num2 != num)
				{
					ProgressBar1.Value += 1;
					long num4 = ProgressBar1.Value;
					((Control)lblProgress).Text = StringType.FromLong(num4) + " / " + StringType.FromLong(num3);
					Application.DoEvents();
					string text = StringType.FromObject(lstRecursive.Items[(int)num2]);
					FindResource(text);
					HandleOBJD(text, 2);
					HandleCTSS(text);
					string text2 = txtGroup.Text;
					text2 = ((StringType.StrCmp(text2, "FFFFFFFF", false) != 0) ? text2 : "00000000");
					if (StringType.StrCmp(txtGUID.Text, "", false) == 0)
					{
						num2++;
						continue;
					}
					lstBatchFileList.Items.Add((object)text);
					string text3 = txtGUID.Text;
					string text4 = "69DA3F9F";
					string text5 = "00000000";
					string text6 = txtCTSSName.Text;
					lstBatchAdd.Items.Add((object)("Object - " + text6 + ": " + text4 + " - " + text5 + " - " + text2 + " - " + text3));
					string text7 = Common.Translate("0x" + text3, Application.StartupPath + "/UserObjectList.txt", 1L, 2L, ';');
					if (StringType.StrCmp(text7, "No Match Found", false) == 0)
					{
						Common.AppendToFile("0x" + text2 + ";0x" + text3 + ";" + text6 + ";;", Application.StartupPath + "/UserObjectList.txt");
					}
					num2++;
				}
			}
			lstDatFileNames.Items.Clear();
			lstRecursive.Items.Clear();
			((Control)GroupBox3).Visible = false;
			((Control)lblBatchAddTotal).Text = "Total Items: " + StringType.FromInteger(lstBatchAdd.Items.Count);
			Interaction.Beep();
			Interaction.Beep();
			Interaction.Beep();
		}
	}

	private void cmdBackUpColl_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		string selectedPath;
		if (StringType.StrCmp(txtCollDir.Text, "", false) == 0)
		{
			FolderBrowserDialog1.Description = "Choose Your Collections Directory";
			((CommonDialog)FolderBrowserDialog1).ShowDialog();
			selectedPath = FolderBrowserDialog1.SelectedPath;
			return;
		}
		selectedPath = txtCollDir.Text;
		checked
		{
			if (StringType.StrCmp(selectedPath, "", false) != 0)
			{
				string[] array = IOTools.GetsFilesNoDir(selectedPath + "/", "*.package");
				for (long num = 0L; num < array.Length; num++)
				{
					FileSystem.FileCopy(selectedPath + "/" + array[(int)num], Application.StartupPath + "/Backups/" + array[(int)num]);
				}
			}
			if (!chkWarningOff.Checked)
			{
				Interaction.MsgBox((object)("Collections backed up! You can find them in this directory:\r\n" + Application.StartupPath + "/Backups/"), (MsgBoxStyle)64, (object)"Collections Succsessfully Backed Up");
			}
		}
	}

	private void cmdBatchAddUp_Click(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (lstBatchAdd.SelectedIndex == -1)
		{
			if (!chkWarningOff.Checked)
			{
				Interaction.MsgBox((object)"You can't move nothing! The program will a splode! Select an Item first!", (MsgBoxStyle)48, (object)"Sims 2 Collection Maker");
			}
			return;
		}
		if (lstBatchAdd.SelectedIndex == 0)
		{
			if (!chkWarningOff.Checked)
			{
				Interaction.MsgBox((object)"You can't move this up anymore! The program will go a splode!", (MsgBoxStyle)64, (object)"Sims 2 Collection Maker");
			}
			return;
		}
		long num = lstBatchAdd.SelectedIndex;
		checked
		{
			string text = StringType.FromObject(lstBatchFileList.Items[(int)num]);
			string text2 = StringType.FromObject(lstBatchFileList.Items[(int)(num - 1L)]);
			lstBatchFileList.Items[(int)num] = text2;
			lstBatchFileList.Items[(int)(num - 1L)] = text;
			lstBatchFileList.SelectedIndex = (int)(num - 1L);
			text = StringType.FromObject(lstBatchAdd.Items[(int)num]);
			text2 = StringType.FromObject(lstBatchAdd.Items[(int)(num - 1L)]);
			lstBatchAdd.Items[(int)num] = text2;
			lstBatchAdd.Items[(int)(num - 1L)] = text;
			lstBatchAdd.SelectedIndex = (int)(num - 1L);
			ChangeSta("Up, up and away!");
			OldStaText = "Up, up and away!";
		}
	}

	private void cmdBatchAddDown_Click(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		if (lstBatchAdd.SelectedIndex == -1)
		{
			if (!chkWarningOff.Checked)
			{
				Interaction.MsgBox((object)"You can't move nothing! The program will a splode! Select an item to move first!", (MsgBoxStyle)48, (object)"Sims 2 Collection Maker");
			}
			return;
		}
		checked
		{
			if (lstBatchAdd.SelectedIndex == lstBatchAdd.Items.Count - 1)
			{
				if (!chkWarningOff.Checked)
				{
					Interaction.MsgBox((object)"You can't move this down anymore! The program will go a splode!", (MsgBoxStyle)64, (object)"Sims 2 Collection Maker");
				}
				return;
			}
			long num = lstBatchAdd.SelectedIndex;
			string text = StringType.FromObject(lstBatchFileList.Items[(int)num]);
			string text2 = StringType.FromObject(lstBatchFileList.Items[(int)(num + 1L)]);
			lstBatchFileList.Items[(int)num] = text2;
			lstBatchFileList.Items[(int)(num + 1L)] = text;
			lstBatchFileList.SelectedIndex = (int)(num + 1L);
			text = StringType.FromObject(lstBatchAdd.Items[(int)num]);
			text2 = StringType.FromObject(lstBatchAdd.Items[(int)(num + 1L)]);
			lstBatchAdd.Items[(int)num] = text2;
			lstBatchAdd.Items[(int)(num + 1L)] = text;
			lstBatchAdd.SelectedIndex = (int)(num + 1L);
			ChangeSta("Oh no... we're going down!");
			OldStaText = "Oh no... we're going down!";
		}
	}

	private void cmdBatchAddRemove_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (lstBatchAdd.SelectedIndex == -1)
		{
			if (!chkWarningOff.Checked)
			{
				Interaction.MsgBox((object)"You can't remove nothing! The program will a splode! Select something to remove first.", (MsgBoxStyle)48, (object)"Sims 2 Collection Maker");
			}
		}
		else
		{
			lstBatchFileList.Items.RemoveAt(lstBatchAdd.SelectedIndex);
			lstBatchAdd.Items.RemoveAt(lstBatchAdd.SelectedIndex);
		}
		ChangeSta("Item removed from batch add list.");
		OldStaText = "Item removed from batch add list.";
	}

	private void cmdCancelBatchAdd_Click(object sender, EventArgs e)
	{
		ChangeSta("Batch add canceled.");
		OldStaText = "Batch add canceled.";
		ShowUI(2L);
	}

	private void cmdFinishBatchAdd_Click(object sender, EventArgs e)
	{
		long num = lstBatchAdd.Items.Count;
		checked
		{
			for (long num2 = 0L; num2 != num; num2++)
			{
				lstListOfItems.Items.Add(RuntimeHelpers.GetObjectValue(lstBatchAdd.Items[(int)num2]));
			}
			ChangeSta("Batch add completed.");
			OldStaText = "Batch add completed.";
			ShowUI(2L);
		}
	}

	private void cmdAlphaSort_Click(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		lstABC1.Sorted = true;
		lstABC1.Items.Clear();
		lstBatchAdd.Items.Clear();
		lstBatchFileList.Items.Clear();
		FolderBrowserDialog1.SelectedPath = "";
		((CommonDialog)FolderBrowserDialog1).ShowDialog();
		if (StringType.StrCmp(FolderBrowserDialog1.SelectedPath, "", false) == 0)
		{
			return;
		}
		string[] array = IOTools.GetsFilesNoDir(FolderBrowserDialog1.SelectedPath + "/", "*.package");
		checked
		{
			for (long num = 0L; num < array.Length; num++)
			{
				lstBatchAdd.Items.Add((object)array[(int)num]);
			}
			long num2 = lstBatchAdd.Items.Count;
			long num3 = 0L;
			HideUI(3L);
			long num4 = lstBatchAdd.Items.Count;
			ProgressBar1.Maximum = (int)(num4 + 1L);
			ProgressBar1.Value = 0;
			ProgressBar1.Minimum = 0;
			((Control)lblProgressDesc).Text = "Analyzing Collections...";
			ChangeSta("Analyzing Collections...");
			OldStaText = "Analyzing Collections...";
			Application.DoEvents();
			while (num3 != num2)
			{
				ProgressBar1.Value += 1;
				long num5 = ProgressBar1.Value;
				((Control)lblProgress).Text = StringType.FromLong(num5) + " / " + StringType.FromLong(num4);
				Application.DoEvents();
				string text = StringType.FromObject(lstBatchAdd.Items[(int)num3]);
				FindResource(FolderBrowserDialog1.SelectedPath + "/" + text);
				if (lstResources.Items.Count == 0)
				{
					num3++;
					continue;
				}
				string text2 = "6C4F359D";
				int count = lstResources.Items.Count;
				int num6 = 0;
				while (true)
				{
					if (num6 == count)
					{
						num3++;
						break;
					}
					if (ObjectType.ObjTst((object)text2, lstResources.Items[num6], false) == 0)
					{
						HandleSTRFile(FolderBrowserDialog1.SelectedPath + "/" + text);
						string text3 = StringType.FromLong(num3);
						if (Strings.Len(text3) == 1)
						{
							text3 = "000" + text3;
						}
						else if (Strings.Len(text3) == 2)
						{
							text3 = "00" + text3;
						}
						else if (Strings.Len(text3) == 3)
						{
							text3 = "0" + text3;
						}
						if (StringType.StrCmp(txtCollName.Text, "", false) == 0)
						{
							num3++;
							break;
						}
						lstABC1.Items.Add((object)(txtCollName.Text + " " + text3));
						txtCollName.Text = "";
						num3++;
						break;
					}
					num6++;
				}
			}
			ChangeSta("Sorting Collections...");
			OldStaText = "Sorting Collections...";
			Interaction.MsgBox((object)"OK! Collection files have been examined and are ready to be sorted! Please note that this can take quite a while, especially if you have a LOT of collection files or if this is the very first time you've sorted them. Please be patient, and don't click on anything in the program while it's working, you might make it freeze!", (MsgBoxStyle)16, (object)"Please Read This Information");
			long num7 = 0L;
			num2 = lstABC1.Items.Count;
			long num8 = lstABC1.Items.Count;
			ProgressBar1.Maximum = (int)(num8 + 1L);
			ProgressBar1.Value = 0;
			ProgressBar1.Minimum = 0;
			((Control)lblProgressDesc).Text = "Resaving Collections in ABC Order";
			Application.DoEvents();
			for (; num7 != num2; num7++)
			{
				ProgressBar1.Value += 1;
				long num9 = ProgressBar1.Value;
				((Control)lblProgress).Text = StringType.FromLong(num9) + " / " + StringType.FromLong(num8);
				Application.DoEvents();
				string text4 = StringType.FromObject(lstABC1.Items[(int)num7]);
				long num10 = LongType.FromString(Strings.Right(text4, 4));
				string text = StringType.FromObject(lstBatchAdd.Items[(int)num10]);
				txtCollName.Text = "";
				cmbCollType.Text = "";
				lstListOfItems.Items.Clear();
				FindResource(FolderBrowserDialog1.SelectedPath + "/" + text);
				if (lstResources.Items.Count == 0)
				{
					continue;
				}
				Handle3IDR(FolderBrowserDialog1.SelectedPath + "/" + text);
				HandleJPGImage(FolderBrowserDialog1.SelectedPath + "/" + text, 2L);
				HandleSTRFile(FolderBrowserDialog1.SelectedPath + "/" + text);
				HandleCOLLFile(FolderBrowserDialog1.SelectedPath + "/" + text);
				FileSystem.Kill(FolderBrowserDialog1.SelectedPath + "/" + text);
				txtPackagePath.Text = FolderBrowserDialog1.SelectedPath + "/" + text;
				lstDatFileNames.Items.Clear();
				lstGroups.Items.Clear();
				lstResources.Items.Clear();
				lstSize.Items.Clear();
				lstOffset.Items.Clear();
				lstInstance.Items.Clear();
				lstInstance2.Items.Clear();
				txtResourceCount.Text = "";
				txtIndexSize.Text = "";
				if (StringType.StrCmp(cmbCollType.Text, "Residential", false) == 0)
				{
					FileSystem.FileCopy(Application.StartupPath + "/Templates/HomeCollectionTemplate.dat", Application.StartupPath + "/COLLFile.dat");
				}
				else if (StringType.StrCmp(cmbCollType.Text, "Community", false) == 0)
				{
					FileSystem.FileCopy(Application.StartupPath + "/Templates/CommCollectionTemplate.dat", Application.StartupPath + "/COLLFile.dat");
				}
				else
				{
					FileSystem.FileCopy(Application.StartupPath + "/Templates/BothCollectionTemplate.dat", Application.StartupPath + "/COLLFile.dat");
				}
				if (StringType.StrCmp(cmbCollType.Text, "Residential", false) == 0)
				{
					string text5 = Conversion.Hex(num7);
					if (StringType.StrCmp(text5, "0", false) == 0)
					{
						text5 = "0000";
					}
					else if (Strings.Len(text5) == 3)
					{
						text5 = "0" + text5;
					}
					else if (Strings.Len(text5) == 2)
					{
						text5 = "00" + text5;
					}
					else if (Strings.Len(text5) == 1)
					{
						text5 = "000" + text5;
					}
					string hexString = Strings.Right(text5, 2);
					string hexString2 = Strings.Left(text5, 2);
					FileStream fileStream = new FileStream(Application.StartupPath + "/COLLFile.dat", FileMode.Open, FileAccess.ReadWrite);
					BinaryWriter binaryWriter = new BinaryWriter(fileStream);
					byte value = (byte)CCB.Math.ToDec(hexString);
					binaryWriter.BaseStream.Position = 153L;
					binaryWriter.Write(value);
					value = (byte)CCB.Math.ToDec(hexString2);
					binaryWriter.Write(value);
					fileStream.Close();
					binaryWriter.Close();
				}
				else if (StringType.StrCmp(cmbCollType.Text, "Both (PETS ONLY)", false) == 0)
				{
					string text5 = Conversion.Hex(num7);
					if (StringType.StrCmp(text5, "0", false) == 0)
					{
						text5 = "0000";
					}
					else if (Strings.Len(text5) == 3)
					{
						text5 = "0" + text5;
					}
					else if (Strings.Len(text5) == 2)
					{
						text5 = "00" + text5;
					}
					else if (Strings.Len(text5) == 1)
					{
						text5 = "000" + text5;
					}
					string hexString = Strings.Right(text5, 2);
					string hexString2 = Strings.Left(text5, 2);
					FileStream fileStream2 = new FileStream(Application.StartupPath + "/COLLFile.dat", FileMode.Open, FileAccess.ReadWrite);
					BinaryWriter binaryWriter2 = new BinaryWriter(fileStream2);
					byte value = (byte)CCB.Math.ToDec(hexString);
					binaryWriter2.BaseStream.Position = 156L;
					binaryWriter2.Write(value);
					value = (byte)CCB.Math.ToDec(hexString2);
					binaryWriter2.Write(value);
					fileStream2.Close();
					binaryWriter2.Close();
				}
				else
				{
					string text5 = Conversion.Hex(num7);
					if (StringType.StrCmp(text5, "0", false) == 0)
					{
						text5 = "0000";
					}
					else if (Strings.Len(text5) == 3)
					{
						text5 = "0" + text5;
					}
					else if (Strings.Len(text5) == 2)
					{
						text5 = "00" + text5;
					}
					else if (Strings.Len(text5) == 1)
					{
						text5 = "000" + text5;
					}
					string hexString = Strings.Right(text5, 2);
					string hexString2 = Strings.Left(text5, 2);
					FileStream fileStream3 = new FileStream(Application.StartupPath + "/COLLFile.dat", FileMode.Open, FileAccess.ReadWrite);
					BinaryWriter binaryWriter3 = new BinaryWriter(fileStream3);
					byte value = (byte)CCB.Math.ToDec(hexString);
					binaryWriter3.BaseStream.Position = 165L;
					binaryWriter3.Write(value);
					value = (byte)CCB.Math.ToDec(hexString2);
					binaryWriter3.Write(value);
					fileStream3.Close();
					binaryWriter3.Close();
				}
				lstDatFileNames.Items.Add((object)"COLLFile.dat");
				string text6 = txtCollID.Text;
				Reverse(text6, 1L);
				lstGroups.Items.Add((object)"FFFFFFFF");
				lstInstance2.Items.Add((object)"00000000");
				long num11 = FileSystem.FileLen(Application.StartupPath + "/COLLFile.dat");
				string filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/COLLFile.dat"));
				Fill(filler, 1L);
				long num12 = 96L;
				string filler2 = Conversion.Hex(num12);
				Fill(filler2, 2L);
				num12 += num11;
				lstResources.Items.Add((object)"9D354F6C");
				FileSystem.FileCopy(txtImgPath.Text, Application.StartupPath + "/CollIcon.dat");
				lstDatFileNames.Items.Add((object)"CollIcon.dat");
				text6 = "00000001";
				Reverse(text6, 1L);
				lstGroups.Items.Add((object)"FFFFFFFF");
				lstInstance2.Items.Add((object)"00000000");
				num11 = FileSystem.FileLen(Application.StartupPath + "/CollIcon.dat");
				filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/CollIcon.dat"));
				Fill(filler, 1L);
				filler2 = Conversion.Hex(num12);
				Fill(filler2, 2L);
				num12 += num11;
				lstResources.Items.Add((object)"ACDB6D85");
				FileSystem.FileCopy(Application.StartupPath + "/Templates/STRTemplate.dat", Application.StartupPath + "/STRFile.dat");
				lstDatFileNames.Items.Add((object)"STRFile.dat");
				text6 = "00000001";
				Reverse(text6, 1L);
				lstGroups.Items.Add((object)"FFFFFFFF");
				lstInstance2.Items.Add((object)"00000000");
				StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "/Temp.txt");
				streamWriter.Write(txtCollName.Text);
				streamWriter.Close();
				FileStream fileStream4 = new FileStream(Application.StartupPath + "/Temp.txt", FileMode.Open, FileAccess.ReadWrite);
				BinaryReader binaryReader = new BinaryReader(fileStream4);
				long num13 = 1L;
				long num14 = FileSystem.FileLen(Application.StartupPath + "/Temp.txt");
				FileStream fileStream5 = new FileStream(Application.StartupPath + "/STRFile.dat", FileMode.Append, FileAccess.Write);
				BinaryWriter binaryWriter4 = new BinaryWriter(fileStream5);
				long num15 = FileSystem.FileLen(Application.StartupPath + "/STRFile.dat");
				byte[] buffer = binaryReader.ReadBytes((int)num14);
				binaryWriter4.Write(buffer);
				byte value2 = 0;
				binaryWriter4.Write(value2);
				binaryWriter4.Write(value2);
				fileStream5.Close();
				binaryWriter4.Close();
				fileStream4.Close();
				binaryReader.Close();
				num11 = FileSystem.FileLen(Application.StartupPath + "/STRFile.dat");
				filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/STRFile.dat"));
				Fill(filler, 1L);
				filler2 = Conversion.Hex(num12);
				Fill(filler2, 2L);
				num12 += num11;
				lstResources.Items.Add((object)"23525453");
				lstDatFileNames.Items.Add((object)"3IDRStrImg.dat");
				FileStream fileStream6 = new FileStream(Application.StartupPath + "/3IDRStrImg.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
				BinaryWriter binaryWriter5 = new BinaryWriter(fileStream6);
				value2 = (byte)CCB.Math.ToDec("EF");
				binaryWriter5.Write(value2);
				value2 = (byte)CCB.Math.ToDec("BE");
				binaryWriter5.Write(value2);
				value2 = (byte)CCB.Math.ToDec("AD");
				binaryWriter5.Write(value2);
				value2 = (byte)CCB.Math.ToDec("DE");
				binaryWriter5.Write(value2);
				value2 = (byte)CCB.Math.ToDec("02");
				binaryWriter5.Write(value2);
				value2 = (byte)CCB.Math.ToDec("00");
				binaryWriter5.Write(value2);
				binaryWriter5.Write(value2);
				binaryWriter5.Write(value2);
				value2 = (byte)CCB.Math.ToDec("02");
				binaryWriter5.Write(value2);
				value2 = (byte)CCB.Math.ToDec("00");
				binaryWriter5.Write(value2);
				binaryWriter5.Write(value2);
				binaryWriter5.Write(value2);
				binaryWriter5.Close();
				fileStream6.Close();
				Reverse("856DDBAC", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("FFFFFFFF", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("00000001", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("00000000", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("53545223", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("FFFFFFFF", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("00000001", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				Reverse("00000000", 4L, Application.StartupPath + "/3IDRStrImg.dat");
				text6 = txtCollID.Text;
				Reverse(text6, 1L);
				lstGroups.Items.Add((object)"FFFFFFFF");
				lstInstance2.Items.Add((object)"00000000");
				num11 = FileSystem.FileLen(Application.StartupPath + "/3IDRStrImg.dat");
				filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/3IDRStrImg.dat"));
				Fill(filler, 1L);
				filler2 = Conversion.Hex(num12);
				Fill(filler2, 2L);
				num12 += num11;
				lstResources.Items.Add((object)"646750AC");
				long num16 = lstListOfItems.Items.Count;
				for (num13 = 0L; num13 != num16; num13++)
				{
					string dataString = StringType.FromObject(lstListOfItems.Items[(int)num13]);
					text6 = txtCollID.Text;
					Make3IDR(dataString, num13, text6);
					lstDatFileNames.Items.Add((object)("3IDRNumber" + StringType.FromLong(num13) + ".dat"));
					num14 = CCB.Math.ToDec(text6);
					num14 = num14 + num13 + 1L;
					text6 = Conversion.Hex(num14);
					Reverse(text6, 1L);
					lstGroups.Items.Add((object)"FFFFFFFF");
					lstInstance2.Items.Add((object)"00000000");
					lstResources.Items.Add((object)"646750AC");
					num11 = FileSystem.FileLen(Application.StartupPath + "/3IDRNumber" + StringType.FromLong(num13) + ".dat");
					filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/3IDRNumber" + StringType.FromLong(num13) + ".dat"));
					Fill(filler, 1L);
					filler2 = Conversion.Hex(num12);
					Fill(filler2, 2L);
					num12 += num11;
					MakeBINX(num13);
					text6 = txtCollID.Text;
					num14 = CCB.Math.ToDec(text6);
					num14 = num14 + num13 + 1L;
					text6 = Conversion.Hex(num14);
					Reverse(text6, 1L);
					lstGroups.Items.Add((object)"FFFFFFFF");
					lstInstance2.Items.Add((object)"00000000");
					lstResources.Items.Add((object)"390F560C");
					num11 = FileSystem.FileLen(Application.StartupPath + "/BINXResource" + StringType.FromLong(num13) + ".dat");
					filler = Conversion.Hex(FileSystem.FileLen(Application.StartupPath + "/BINXResource" + StringType.FromLong(num13) + ".dat"));
					Fill(filler, 1L);
					filler2 = Conversion.Hex(num12);
					Fill(filler2, 2L);
					num12 += num11;
				}
				MakeHeader(StringType.FromLong(num12), StringType.FromInteger(lstDatFileNames.Items.Count), StringType.FromInteger(lstDatFileNames.Items.Count * 20));
				MakeIndex();
				CombineDats();
				if (chkCompression.Checked)
				{
					startProcess(Application.StartupPath + "/dbpf-recompress.exe", FolderBrowserDialog1.SelectedPath + "/" + text);
				}
			}
			Interaction.Beep();
			Interaction.Beep();
			Interaction.Beep();
			ChangeSta("Alphabetical sort operation finished!");
			OldStaText = "Alphabetical sort operation finished!";
			lstListOfItems.Items.Clear();
			txtCollName.Text = "";
			cmbCollType.Text = "";
			ShowUI(3L);
		}
	}

	private void btnDEBUG_Click(object sender, EventArgs e)
	{
		PullThumbnail("test");
	}

	private object Recursive(string strPath)
	{
		string[] files = IOTools.GetFiles(strPath + "/", "*.package", includeSubFolders: true);
		checked
		{
			for (long num = 0L; num < files.Length; num++)
			{
				lstRecursive.Items.Add((object)files[(int)num]);
			}
			object result = default(object);
			return result;
		}
	}

	private object HideUI(long GroupBoxNumber)
	{
		((Control)Command1).Visible = false;
		((Control)cmdExit).Visible = false;
		((Control)cmdMakeNewColl).Visible = false;
		((Control)cmdLoadPic).Visible = false;
		((Control)cmdSaveColl).Visible = false;
		((Control)cmdAlphaSort).Visible = false;
		((Control)cmdOptions).Visible = false;
		((Control)cmdAbout).Visible = false;
		((Control)Label5).Visible = false;
		((Control)Label6).Visible = false;
		((Control)Label7).Visible = false;
		((Control)Picture1).Visible = false;
		((Control)txtImgPath).Visible = false;
		((Control)txtCollID).Visible = false;
		((Control)txtCollName).Visible = false;
		((Control)lstListOfItems).Visible = false;
		((Control)cmdMoveUp).Visible = false;
		((Control)cmdMoveDown).Visible = false;
		((Control)cmdRemoveItem).Visible = false;
		((Control)cmdBatchAdd).Visible = false;
		((Control)cmdBackUpColl).Visible = false;
		((Control)cmdEditColl).Visible = false;
		((Control)cmbCollType).Visible = false;
		switch (GroupBoxNumber)
		{
		case 1L:
			((Control)GroupBox1).Visible = true;
			break;
		case 2L:
			((Control)GroupBox2).Visible = true;
			break;
		case 3L:
			((Control)GroupBox3).Visible = true;
			break;
		case 4L:
			((Control)GroupBox4).Visible = true;
			break;
		}
		object result = default(object);
		return result;
	}

	private object ShowUI(long GroupBoxNumber)
	{
		((Control)Command1).Visible = true;
		((Control)cmdExit).Visible = true;
		((Control)cmdMakeNewColl).Visible = true;
		((Control)cmdLoadPic).Visible = true;
		((Control)cmdSaveColl).Visible = true;
		((Control)cmdAlphaSort).Visible = true;
		((Control)cmdOptions).Visible = true;
		((Control)cmdAbout).Visible = true;
		((Control)Label5).Visible = true;
		((Control)Label6).Visible = true;
		((Control)Label7).Visible = true;
		((Control)Picture1).Visible = true;
		((Control)txtImgPath).Visible = true;
		((Control)txtCollID).Visible = true;
		((Control)txtCollName).Visible = true;
		((Control)lstListOfItems).Visible = true;
		((Control)cmdMoveUp).Visible = true;
		((Control)cmdMoveDown).Visible = true;
		((Control)cmdRemoveItem).Visible = true;
		((Control)cmdBatchAdd).Visible = true;
		((Control)cmdBackUpColl).Visible = true;
		((Control)cmdEditColl).Visible = true;
		((Control)cmbCollType).Visible = true;
		switch (GroupBoxNumber)
		{
		case 1L:
			((Control)GroupBox1).Visible = false;
			break;
		case 2L:
			((Control)GroupBox2).Visible = false;
			break;
		case 3L:
			((Control)GroupBox3).Visible = false;
			break;
		case 4L:
			((Control)GroupBox4).Visible = false;
			break;
		}
		object result = default(object);
		return result;
	}

	private object ChangeSta(string Text)
	{
		StatusBar1.Panels[0].Text = Text;
		object result = default(object);
		return result;
	}

	private void lstBatchCategories_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (lstBatchCategories.SelectedIndex > -1)
		{
			ChangeSta(StringType.FromObject(lstBatchCategories.Items[lstBatchCategories.SelectedIndex]));
		}
	}

	private void cmdAbout_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdOptions_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdAbout_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("About this Program");
	}

	private void cmdOptions_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Options");
	}

	private void cmdAlphaSort_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Alphabetical Sorter");
	}

	private void cmdBackUpColl_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Back Up Existing Collections");
	}

	private void cmdEditColl_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Edit Existing Collection");
	}

	private void cmdMakeNewColl_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Make New Collection");
	}

	private void cmdLoadPic_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Load an Icon for the Collection");
	}

	private void Command1_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Add a Single Object to This Collection");
	}

	private void cmdBatchAdd_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Batch Add Contents of a Folder to This Collection");
	}

	private void cmdMoveUp_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Move This Item Up in Collection");
	}

	private void cmdRemoveItem_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Remove This Item From This Collection");
	}

	private void cmdMoveDown_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Move This Item Down in Collection");
	}

	private void cmdExit_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Exit");
	}

	private void cmdSaveColl_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Save this Collection");
	}

	private void cmdFindCollDir_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Pick a Directory");
	}

	private void cmdFindThumbDir_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Pick a Directory");
	}

	private void cmdCloseOptions_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Close and Save Changes to Options");
	}

	private void cmdAddItem_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Add this Item to Collection");
	}

	private void cmdCancel_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Cancel; Don't Add this Item to Collection");
	}

	private void cmdFinishBatchAdd_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Add these Items to Collection");
	}

	private void cmdCancelBatchAdd_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Cancel; Don't Add these Items to Collection");
	}

	private void cmdBatchAddUp_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Move Item up in Batch Add List");
	}

	private void cmdBatchAddDown_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Move Item down in Batch Add List");
	}

	private void cmdBatchAddRemove_MouseEnter(object sender, EventArgs e)
	{
		OldStaText = StatusBar1.Panels[0].Text;
		ChangeSta("Remove this Item from Batch Add List");
	}

	private void cmdAlphaSort_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdBackUpColl_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdMoveUp_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdRemoveItem_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdMoveDown_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdExit_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdSaveColl_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdFindCollDir_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdFindThumbDir_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdCloseOptions_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdAddItem_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdCancel_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdFinishBatchAdd_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdCancelBatchAdd_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdBatchAddUp_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdBatchAddDown_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdBatchAddRemove_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdMakeNewColl_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void Command1_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdBatchAdd_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdLoadPic_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void cmdEditColl_MouseLeave(object sender, EventArgs e)
	{
		ChangeSta(OldStaText);
	}

	private void startProcess(string Path, string fileName)
	{
		Process process = new Process();
		process.StartInfo.Arguments = "\"" + fileName + "\"";
		process.StartInfo.FileName = Path;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.Start();
	}

	private void lstBatchAdd_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (lstBatchAdd.SelectedIndex != -1)
		{
			PullThumbnail(StringType.FromObject(lstBatchFileList.Items[lstBatchAdd.SelectedIndex]), 2);
		}
	}

	private void lstInstance_SizeChanged(object sender, EventArgs e)
	{
		ThumbPackageLoaded = 0;
	}
}
