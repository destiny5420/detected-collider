# Collision Deteted By Perspective Camera

由於公司的專案是屬於2D類型的遊戲，也就使用了Unity中的BoxCollider2D來做偵測判斷，攝影機的方面，也選擇了正投影(Orthographic)模式來使用。不過因想嘗試著導入3D模組，也讓匯入的模型能有透視的感覺，因此也將攝影機改為透視(Perspective)模式。
<br>
<br>
因專案的特性，必須保與2D物件有深度(depth)的特性，且搭配著透視攝影機來製作，而讓我們最先面臨到的問題是<br>
<strong>「玩家看到物件與物件碰撞了，但碰撞塊卻沒碰撞到」</strong>

<br>
<p align="left">
在攝影機為Orthographic模式下的狀態
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_1.png">
</p>
<p align="center"><em>圖 1-1. Orthographic Camera</em></p>

<br>
<p align="left">
在攝影機為Perspective模式下的狀態
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_2.png">
</p>
<p align="center"><em>圖 1-2. Perspective Camera</em></p>

<br>
在圖1-1中可見，原本使用BoxCollider2D在正投影攝影機下是沒問題的，但是在圖1-2的透視攝影機中，因透視的關係，導致玩家在視覺上已經看到物件碰撞，但在物理碰撞上其實尚未碰撞
<br>
<br>
<br>

## 解決方式
1. 從攝影機打出一條射線，通過物件的其中一個點延伸至一個虛擬平面上
2. 將物件延伸至虛擬平面上的所有點成一個偵測範圍
3. 使用 <em>**分離軸定理(Separating Axis Theorem，簡稱SAT)**</em> 來做碰撞判斷

<br>
<p align="left">
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_3.png">
</p>
<p align="center"><em>圖 1-3. projection of plane</em></p>

<br>

<p align="left">
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_4.png">
</p>
<p align="center"><em>圖 1-4. projection of plane</em></p>

因此我們必須先求得一個點在虛擬平面上的位置(藍點)，其餘的點遵照此方法就可以求得了<br>

在圖1-4中，紫色的點是自行設置類似BoxCollider的點，在此專案中設置了四個
```C#
public class DetectCollider : MonoBehaviour
{
  // ... 省略 ...
  [SerializeField] Vector3[] m_v3AryPoint; // 起始時設置紫色點的位置
  udsPointData[] m_sttObjPointData; // Update紫色點位置
  Vector3[] m_v3VecA; // 儲存向量A
}
```

紅色的點則是攝影機垂直向下到虛擬平面的點，因此我設置在(0, 0, 0)的位置上z
<br>
<br>

* 首先我們可以透過PointData與Camera位置取得向量A
```C#
m_v3VecA[i] = m_sttObjPointData[i].point - m_camera.transform.position
```

<br>

* 可以透過(0, 0, 0)與Camera位置取得向量B
```C#
m_v3VecB[i] = m_v3CenterVec - m_camera.transform.position;
```

<br>

* 接下來是要想辦法取得**灰點**到**黃點**的長度，也就是 <a href="https://www.codecogs.com/eqnedit.php?latex=\dpi{120}&space;\large&space;$\lvert\vec{D}\rvert$" target="_blank"><img src="https://latex.codecogs.com/png.latex?\dpi{120}&space;\large&space;$\lvert\vec{D}\rvert$" title="\large $\lvert\vec{D}\rvert$" /></a>
