
package com.unity3d.player;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.ActivityInfo;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.webkit.WebResourceError;
import android.webkit.WebResourceRequest;
import android.webkit.WebView;
import android.webkit.WebViewClient;
 
public class PrivacyActivity extends Activity implements DialogInterface.OnClickListener {
    boolean useLocalHtml = true;
    String privacyUrl = "https://www.taptap.cn/";
    final String htmlStr = "欢迎使用本游戏，在使用本游戏前，请您充分阅读并理解下面的协议各条<br>" +
            "款，了解我们对于个人信息的处理规则和权限申请的目的，特别提醒您注意前述协议中关于<br>" +
            "我们免除自身责任，限制您的权力的相关条款及争议解决方式，司法管辖等内容。我们将严<br>" +
            "格遵守相关法律法规和隐私政策以保护您的个人隐私。为确保您的游戏体验，我们会向您申请以下必要权限，您可选择同意或者拒绝，拒绝可能会导致无法进入本游戏。同时，我们会根据本游戏中相关功能的具体需要向您申请非必要的权限，您可选择同意或者拒绝，拒绝可能会导致部分游戏体验异常。其中必要权限包括：设备权限(必要)：读取唯一设备标识 (AndroidID、mac)，生成帐号、保存和恢复游戏数据，识别异常状态以及保障网络及运营安全。存储权限(必要)：访问您的存储空间，以便使您可以下载并保存内容、图片存储及上传、个人设置信息缓存读写、系统及日志文件创建。<br>"+
            "<br><br>具体协议如下<br><br>"+
"《化光而逝隐私政策》<br>"+
"更新日期：2023年9月13日<br>"+
"生效日期：2023年9月13日<br>"+
"提示条款<br>"+
"欢迎您使用化光而逝！我们非常重视保护您的个人信息和隐私。您可以通过《化光而逝隐私政策》了解我们收集、使用、存储用户个人信息的情况，以及您所享有的相关权利。请您仔细阅读并充分理解相关内容：<br>"+
"<br>"+
"1. 适用范围<br>"+
"    在您使用本软件服务，本软件会读取您的网络状态用于可能加载的协议地址。<br>"+
"2. 信息的使用<br>"+
"    在获得您的数据之后，本软件不会将其上传至服务器或者储存，只用于游戏内。<br>"+
"3. 信息披露         <br>"+
"    本软件不会将您的信息披露给不受信任的第三方；<br>"+
"    根据法律的有关规定，或者行政或司法机构的要求，向第三方或者行政、司法机构披露；<br>"+
"    如您出现违反中国有关法律、法规或者相关规则的情况，需要向第三方披露；<br>"+
"4. 信息存储和交换<br>"+
"    本软件不存储收集的有关您的信息和资料 。<br>"+
"5. 第三方 SDK 名称：Unity 3D<br>"+
"    应用场景： 框架<br>"+
"    可能收集的个人信息的类型：读取设备信息（设备型号、系统名称、系统版本、MAC 地址、IMEI、Android ID）、访问相机、应用安装列表、任务列表、网络状态信息、设备传感器列表<br>"+
"    第三方 SDK 提供方：Unity Technologies<br>"+
"    隐私政策链接：<a href=\"https://unity.com/cn/legal/privacy-policy\">https://unity.com/cn/legal/privacy-policy</a><br>"+
"<br>"+
"本政策仅用于 旁观以待 游戏服务<br>"+
"如果您有疑问、意见或建议，请通过以下联系方式与我们联系：<br>"+
"电子邮件：1430791209@qq.com  <br>"+
"电话：13333738416";
 
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
 
        ActivityInfo actInfo = null;
        try {
            //获取AndroidManifest.xml配置的元数据
            actInfo = this.getPackageManager().getActivityInfo(getComponentName(), PackageManager.GET_META_DATA);
            useLocalHtml = actInfo.metaData.getBoolean("useLocalHtml");
            privacyUrl = actInfo.metaData.getString("privacyUrl");
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
 
        //如果已经同意过隐私协议则直接进入Unity Activity
        if (GetPrivacyAccept()){
            EnterUnityActivity();
            return;
        }
        ShowPrivacyDialog();//弹出隐私协议对话框
    }
 
    @Override
    public void onClick(DialogInterface dialogInterface, int i) {
        switch (i){
            case AlertDialog.BUTTON_POSITIVE://点击同意按钮
                SetPrivacyAccept(true);
                EnterUnityActivity();//启动Unity Activity
                break;
            case AlertDialog.BUTTON_NEGATIVE://点击拒绝按钮,直接退出App
                finish();
                break;
        }
    }
    private void ShowPrivacyDialog(){
        WebView webView = new WebView(this);
        if (useLocalHtml){
            webView.loadDataWithBaseURL(null, htmlStr, "text/html", "UTF-8", null);
        }else{
            webView.loadUrl(privacyUrl);
            webView.setWebViewClient(new WebViewClient(){
                @Override
                public boolean shouldOverrideUrlLoading(WebView view, String url) {
                    view.loadUrl(url);
                    return true;
                }
 
                @Override
                public void onReceivedError(WebView view, WebResourceRequest request, WebResourceError error) {
                    view.reload();
                }
 
                @Override
                public void onPageFinished(WebView view, String url) {
                    super.onPageFinished(view, url);
                }
            });
        }
 
        AlertDialog.Builder privacyDialog = new AlertDialog.Builder(this);
        privacyDialog.setCancelable(false);
        privacyDialog.setView(webView);
        privacyDialog.setTitle("User Terms & Privacy");
        privacyDialog.setNegativeButton("Exit",this);
        privacyDialog.setPositiveButton("Agree",this);
        privacyDialog.create().show();
    }
//启动Unity Activity
    private void EnterUnityActivity(){
        Intent unityAct = new Intent();
        unityAct.setClassName(this, "com.unity3d.player.UnityPlayerActivity");
        unityAct.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        this.startActivity(unityAct);
    }
//保存同意隐私协议状态
    private void SetPrivacyAccept(boolean accepted){
        SharedPreferences.Editor prefs = this.getSharedPreferences("PlayerPrefs", MODE_PRIVATE).edit();
        prefs.putBoolean("PrivacyAccepted", accepted);
        prefs.apply();
    }
    private boolean GetPrivacyAccept(){
        SharedPreferences prefs = this.getSharedPreferences("PlayerPrefs", MODE_PRIVATE);
        return prefs.getBoolean("PrivacyAccepted", false);
    }
}