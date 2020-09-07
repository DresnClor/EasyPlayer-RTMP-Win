﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyPlayer_RTMP.NetSDK
{
    public class PlayerSdk
    {
        #region 数据结构

        /// <summary>
        /// 编码格式
        /// </summary>
        public enum RENDER_FORMAT
        {

            /// DISPLAY_FORMAT_YV12 -> 842094169
            DISPLAY_FORMAT_YV12 = 842094169,

            /// DISPLAY_FORMAT_YUY2 -> 844715353
            DISPLAY_FORMAT_YUY2 = 844715353,

            /// DISPLAY_FORMAT_UYVY -> 1498831189
            DISPLAY_FORMAT_UYVY = 1498831189,

            /// DISPLAY_FORMAT_A8R8G8B8 -> 21
            DISPLAY_FORMAT_A8R8G8B8 = 21,

            /// DISPLAY_FORMAT_X8R8G8B8 -> 22
            DISPLAY_FORMAT_X8R8G8B8 = 22,

            /// DISPLAY_FORMAT_RGB565 -> 23
            DISPLAY_FORMAT_RGB565 = 23,

            /// DISPLAY_FORMAT_RGB555 -> 25
            DISPLAY_FORMAT_RGB555 = 25,

            /// DISPLAY_FORMAT_RGB24_GDI -> 26
            DISPLAY_FORMAT_RGB24_GDI = 26,
        }

        /// <summary>
        /// 帧结构信息
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct EASY_FRAME_INFO
        {
            public uint codec;                  /* 音视频格式 */

            public uint type;                   /* 视频帧类型 */
            public byte fps;                    /* 视频帧率 */
            public ushort width;               /* 视频宽 */
            public ushort height;              /* 视频高 */

            public uint reserved1;         /* 保留参数1 */
            public uint reserved2;         /* 保留参数2 */

            public uint sample_rate;       /* 音频采样率 */
            public uint channels;          /* 音频声道数 */
            public uint bits_per_sample;        /* 音频采样精度 */

            public uint length;                /* 音视频帧大小 */
            public uint timestamp_usec;        /* 时间戳,微妙 */
            public uint timestamp_sec;          /* 时间戳 秒 */

            public float bitrate;                       /* 比特率 */
            public float losspacket;                    /* 丢包率 */
        }

        /// <summary>
        /// 坐标
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct Point
        {
            /// LONG->int
            public int x;

            /// LONG->int
            public int y;
        }

        /// <summary>
        /// 像素信息
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tagRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// OSD 信息集合
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct EASY_PALYER_OSD
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string stOSD;
            public uint alpha;        //0-255
            public uint color;        //RGB(0xf9,0xf9,0xf9)
            public uint shadowcolor;      //RGB(0x4d,0x4d,0x4d) 全为0背景透明
            public tagRECT rect;      //OSD基于图像右上角显示区域
            public int size;       //just D3D Support
        }

        #endregion

        /// <summary>
        /// 流回调
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.通道ID.</param>
        /// <param name="userPtr">通道指针.</param>
        /// <param name="_frameType">帧数据类型.</param>
        /// <param name="pBuf">数据指针.</param>
        /// <param name="_frameInfo">帧数据结构体.</param>
        /// <returns>System.Int32.</returns>
        public delegate int MediaSourceCallBack(int _channelId, IntPtr _channelPtr, int _frameType, IntPtr pBuf, ref EASY_FRAME_INFO _frameInfo);

        /// <summary>
        /// EasyPlayer初始化.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_Init@@YAHPAD@Z")]
        public static extern int EasyPlayer_Init(string key = "59615A67426F69576B5A7341706B56666F4C705A3065314659584E35556C524E55454E73615756756443356C65475745567778576F502F522F32566863336B3D");

        /// <summary>
        /// 资源释放.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "?EasyPlayer_Release@@YAXXZ")]
        public static extern void EasyPlayer_Release();

        /// <summary>
        /// 开始进行流播放.
        /// </summary>
        /// <param name="url">媒体地址.</param>
        /// <param name="hWnd">窗口句柄.</param>
        /// <param name="renderFormat">编码格式.</param>
        /// <param name="rtpovertcp">拉取流的传输模式，0=udp,1=tcp.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="userPtr">用户自定义指针.</param>
        /// <param name="callback">数据回调.</param>
        /// <param name="bHardDecode">硬件解码1=是，0=否.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_OpenStream@@YAHPBDPAUHWND__@@W4__RENDER_FORMAT@@H00P6GHHPAHHPADPAUEASY_FRAME_INFO@@@ZPAX_N@Z")]
        public static extern int EasyPlayer_OpenStream(string url, IntPtr hWnd, RENDER_FORMAT renderFormat, int rtpovertcp, string username, string password, MediaSourceCallBack callback, IntPtr userPtr, bool bHardDecode = true);

        /// <summary>
        /// 停止取流.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_CloseStream@@YAXH@Z")]
        public static extern int EasyPlayer_CloseStream(int channelId);

        /// <summary>
        /// 设置当前流播放缓存帧数.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="cache">缓存的视频帧数.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetFrameCache@@YAHHH@Z")]
        public static extern int EasyPlayer_SetFrameCache(int channelId, int cache);

        /// <summary>
        /// 播放器按比例进行显示.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="shownToScale">0=整个窗口区域显示，1=按比例显示.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetShownToScale@@YAHHH@Z")]
        public static extern int EasyPlayer_SetShownToScale(int channelId, int shownToScale);

        /// <summary>
        /// 设置是否显示码流信息.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="show">0=不显示，1=显示</param>
        /// <returns></returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_ShowStatisticalInfo@@YAHHH@Z")]
        public static extern int EasyPlayer_ShowStatisticalInfo(int channelId, int show);

        /// <summary>
        /// 开始播放音频.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_PlaySound@@YAHH@Z")]
        public static extern int EasyPlayer_PlaySound(int channelId);

        /// <summary>
        /// 停止播放音频.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StopSound@@YAHXZ")]
        public static extern int EasyPlayer_StopSound();

        /// <summary>
        /// 截图,当前[2017/11/29]仅支持YUV2编码格式下截图
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="shotPath">默认程序路径下shotPath文件夹</param>
        /// <returns></returns>
        public static int EasyPlayer_PicShot(int channelId, string shotPath = null)
        {
            int ret = -99;
            string path = shotPath ?? System.AppDomain.CurrentDomain.BaseDirectory + "shotPath\\";

            if (!System.IO.Directory.Exists(path))//如果不存在就创建file文件夹　　             　　                
                System.IO.Directory.CreateDirectory(path);//创建该文件夹

            ret = EasyPlayer_SetManuPicShotPath(channelId, path);
            ret = EasyPlayer_StartManuPicShot(channelId);
            System.Threading.Thread.Sleep(200);
            ret = EasyPlayer_StopManuPicShot(channelId);
            return ret;
        }

        /// <summary>
        /// 設置截圖的路徑.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="shotPath">The shot path.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetManuPicShotPath@@YAHHPBD@Z")]
        private static extern int EasyPlayer_SetManuPicShotPath(int channelId, string shotPath);

        /// <summary>
        /// Easies the player_ start manu pic shot.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StartManuPicShot@@YAHH@Z")]
        private static extern int EasyPlayer_StartManuPicShot(int channelId);

        /// <summary>
        /// 停止截圖.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StopManuPicShot@@YAHH@Z")]
        private static extern int EasyPlayer_StopManuPicShot(int channelId);

        /// <summary>
        /// 音视频数据录制，录制格式为MP4.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="recordPath">默认程序路径</param>
        /// <returns></returns>
        public static int EasyPlayer_StartManuRecording(int channelId, string recordPath = null)
        {
            int ret = -99;
            string path = recordPath ?? System.AppDomain.CurrentDomain.BaseDirectory + "record\\";

            if (!System.IO.Directory.Exists(path))//如果不存在就创建file文件夹　　             　　                
                System.IO.Directory.CreateDirectory(path);//创建该文件夹
            ret = EasyPlayer_SetManuRecordPath(channelId, path);
            ret = EasyPlayer_StartManuRecording(channelId);
            return ret;
        }

        /// <summary>
        /// 音视频数据录制，录制格式为MP4.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StartManuRecording@@YAHH@Z")]
        private static extern int EasyPlayer_StartManuRecording(int channelId);

        /// <summary>
        /// 設置視頻錄製路徑.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="recordPath">The record path.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetManuRecordPath@@YAHHPBD@Z")]
        private static extern int EasyPlayer_SetManuRecordPath(int channelId, string recordPath);

        /// <summary>
        /// 停止录制.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StopManuRecording@@YAHH@Z")]
        public static extern int EasyPlayer_StopManuRecording(int channelId);

        /// <summary>
        /// 附件文字显示.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="show">1显示，0不显示.</param>
        /// <param name="osdInfo">设置渲染区域的矩形结构体指针.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTMP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_ShowOSD@@YAHHHUtagEASY_PALYER_OSD@@@Z")]
        private static extern int EasyPlayer_ShowOSD(int channelId, int show, EASY_PALYER_OSD osdInfo);

        /// <summary>
        /// 附件文字显示.
        /// </summary>
        /// <param name="channelId">通道ID，EasyPlayer_OpenStream函数返回值.</param>
        /// <param name="isShow">是否显示，默认不显示.</param>
        /// <param name="lpSrcRect">设置渲染区域的矩形结构体.</param>
        /// <returns>System.Int32.</returns>
        public static int EasyPlayer_ShowOSD(int channelId, EASY_PALYER_OSD osdInfo, bool isShow = true)
        {
            int ret = EasyPlayer_ShowOSD(channelId, isShow ? 1 : 0, osdInfo);
            return ret;
        }
    }
}
