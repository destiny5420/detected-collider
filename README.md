# Collision Deteted By Perspective Camera

由於公司的專案是屬於2D類型的遊戲，也就使用了Unity中的BoxCollider2D來做偵測判斷，攝影機的方面，也選擇了正投影(Orthographic)模式來使用

不過因想嘗試著導入3D模組，也讓匯入的模型能有透視的感覺，因此也將攝影機改為透視(Perspective)模式。

而讓我們最先面臨到的問題 ***玩家看到物件與物件碰撞了，但碰撞塊卻沒碰撞到***

<p align="left">
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/image_01.png">
</p>

<p align="center">
這是一般2D攝影機看到的狀態
</p>
