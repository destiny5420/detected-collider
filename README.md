# Collision Deteted By Perspective Camera

由於公司的專案是屬於2D類型的遊戲，也就使用了Unity中的BoxCollider2D來做偵測判斷，攝影機的方面，也選擇了正投影(Orthographic)模式來使用。不過因想嘗試著導入3D模組，也讓匯入的模型能有透視的感覺，因此也將攝影機改為透視(Perspective)模式。

因專案的特性，必須保與2D物件有深度(depth)的特性，且搭配著透視攝影機來製作
而讓我們最先面臨到的問題就是 <br>
<strong>「玩家看到物件與物件碰撞了，但碰撞塊卻沒碰撞到」</strong>

<br>
<p align="left">
在攝影機為Orthographic模式下的狀態
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_1.png">
</p>
<p align="center"><em>圖1-1 Orthographic Camera</em></p>

<br>
<p align="left">
在攝影機為Perspective模式下的狀態
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_2.png">
</p>
<p align="center"><em>圖1-2 Perspective Camera</em></p>

<br>
在圖1-1中可見，原本使用BoxCollider2D在正投影攝影機下是沒問題的，但是在圖1-2的透視攝影機中，因透視的關係，導致玩家在視覺上已經看到物件碰撞，但在物理碰撞上其實尚未碰撞
<br>
<br>
<br>

## 解決方式
1. 從攝影機打出一條射線，通過物件的其中一個點延伸至一個虛擬平面上
2. 將物件延伸至虛擬平面上的所有點成一個偵測範圍
3. 使用 <em>**分離軸定理**</em> 來做碰撞判斷

<br>
<p align="left">
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_3.png">
</p>
<p align="center"><em>圖1-3 projection of plane</em></p>

<br>

$$
\frac
{(\vec{A}\cdot\vec{B})\vec{B}}
{\lvert\vec{B}\rvert^2}
$$
