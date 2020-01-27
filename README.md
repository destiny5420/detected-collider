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

紅色的點則是攝影機垂直向下到虛擬平面的點，因此我設置在(0, 0, 0)的位置上
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

## 取得向量D
<p align="left">
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_5.png">
</p>
<p align="center"><em>圖 1-5. 取得向量D</em></p>

<br>
接下來是要想辦法取得**灰點**到**黃點**的向量，也就是<a href="https://www.codecogs.com/eqnedit.php?latex=$$\vec{D}$$" target="_blank"><img src="https://latex.codecogs.com/gif.latex?$$\vec{D}$$" title="$$\vec{D}$$" /></a>，在這邊必須使用內積<a href="https://www.codecogs.com/eqnedit.php?latex=\dpi{120}&space;\large&space;$$\vec{a}&space;\cdot&space;\vec{b}$$" target="_blank"><img src="https://latex.codecogs.com/png.latex?\dpi{120}&space;\large&space;$$\vec{a}&space;\cdot&space;\vec{b}$$" title="\large $$\vec{a} \cdot \vec{b}$$" /></a>來求得。<br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$&space;\vec{A}&space;\cdot&space;\vec{B}&space;=&space;\left&space;|&space;\vec{A}&space;\right&space;|\left&space;|&space;\vec{B}&space;\right&space;|\cos&space;\theta&space;$$" target="_blank"><img src="https://latex.codecogs.com/gif.latex?$$&space;\vec{A}&space;\cdot&space;\vec{B}&space;=&space;\left&space;|&space;\vec{A}&space;\right&space;|\left&space;|&space;\vec{B}&space;\right&space;|\cos&space;\theta&space;$$" title="$$ \vec{A} \cdot \vec{B} = \left | \vec{A} \right |\left | \vec{B} \right |\cos \theta $$" /></a><br><br>
經過移動之後<br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$\cos&space;\theta&space;=&space;\frac{&space;\vec{A}&space;\cdot&space;\vec{B}&space;}{\left&space;|&space;\vec{A}&space;\right&space;|\left&space;|&space;\vec{B}&space;\right&space;|}$$" target="_blank"><img src="https://latex.codecogs.com/gif.latex?$$\cos&space;\theta&space;=&space;\frac{&space;\vec{A}&space;\cdot&space;\vec{B}&space;}{\left&space;|&space;\vec{A}&space;\right&space;|\left&space;|&space;\vec{B}&space;\right&space;|}$$" title="$$\cos \theta = \frac{ \vec{A} \cdot \vec{B} }{\left | \vec{A} \right |\left | \vec{B} \right |}$$" /></a><br>
且<br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$\cos&space;\theta&space;=&space;\frac{\boldsymbol{x}}{\boldsymbol{r}}=&space;\frac{\left&space;|&space;\vec{D}&space;\right&space;|}{\left&space;|&space;\vec{A}&space;\right&space;|}$$" target="_blank"><img src="https://latex.codecogs.com/gif.latex?$$\cos&space;\theta&space;=&space;\frac{\boldsymbol{x}}{\boldsymbol{r}}=&space;\frac{\left&space;|&space;\vec{D}&space;\right&space;|}{\left&space;|&space;\vec{A}&space;\right&space;|}$$" title="$$\cos \theta = \frac{\boldsymbol{x}}{\boldsymbol{r}}= \frac{\left | \vec{D} \right |}{\left | \vec{A} \right |}$$" /></a><br><br>
因此可以得到<br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$&space;\frac{\vec{A}&space;\cdot&space;\vec{B}}{\left&space;|&space;\vec{A}&space;\right&space;|\left&space;|&space;\vec{B}&space;\right&space;|}&space;=&space;\frac{\left&space;|&space;\vec{D}&space;\right&space;|}{\left&space;|&space;\vec{A}&space;\right&space;|}&space;$$" target="_blank"><img src="https://latex.codecogs.com/png.latex?$$&space;\frac{\vec{A}&space;\cdot&space;\vec{B}}{\left&space;|&space;\vec{A}&space;\right&space;|\left&space;|&space;\vec{B}&space;\right&space;|}&space;=&space;\frac{\left&space;|&space;\vec{D}&space;\right&space;|}{\left&space;|&space;\vec{A}&space;\right&space;|}&space;$$" title="$$ \frac{\vec{A} \cdot \vec{B}}{\left | \vec{A} \right |\left | \vec{B} \right |} = \frac{\left | \vec{D} \right |}{\left | \vec{A} \right |} $$" /></a><br><br>
然後同乘上A向量長度<br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$&space;\frac{\left&space;|&space;\vec{D}&space;\right&space;|&space;\color{Red}\left&space;|&space;{\vec{A}}&space;\right|}{\left&space;|&space;\vec{A}&space;\right&space;|}&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})&space;\color{Red}\left&space;|&space;\vec{A}&space;\right|&space;}{\left&space;|&space;\vec{A}&space;\right&space;|\left&space;|&space;\vec{B}&space;\right&space;|}&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})}{\left&space;|&space;\vec{B}&space;\right&space;|}&space;$$" target="_blank"><img src="https://latex.codecogs.com/gif.latex?$$&space;\frac{\left&space;|&space;\vec{D}&space;\right&space;|&space;\color{Red}\left&space;|&space;{\vec{A}}&space;\right|}{\left&space;|&space;\vec{A}&space;\right&space;|}&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})&space;\color{Red}\left&space;|&space;\vec{A}&space;\right|&space;}{\left&space;|&space;\vec{A}&space;\right&space;|\left&space;|&space;\vec{B}&space;\right&space;|}&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})}{\left&space;|&space;\vec{B}&space;\right&space;|}&space;$$" title="$$ \frac{\left | \vec{D} \right | \color{Red}\left | {\vec{A}} \right|}{\left | \vec{A} \right |} = \frac{(\vec{A} \cdot \vec{B}) \color{Red}\left | \vec{A} \right| }{\left | \vec{A} \right |\left | \vec{B} \right |} = \frac{(\vec{A} \cdot \vec{B})}{\left | \vec{B} \right |} $$" /></a><br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$\left&space;|&space;\vec{D}&space;\right|&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})}{\left&space;|&space;\vec{B}&space;\right&space;|}$$" target="_blank"><img src="https://latex.codecogs.com/gif.latex?$$\left&space;|&space;\vec{D}&space;\right|&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})}{\left&space;|&space;\vec{B}&space;\right&space;|}$$" title="$$\left | \vec{D} \right| = \frac{(\vec{A} \cdot \vec{B})}{\left | \vec{B} \right |}$$" /></a><br><br>
此時得到的是向量D的長度，並不是向量。因此只要乘上向量B的單位向量，就可以獲得向量D<br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$&space;\hat{B}&space;=&space;\frac{\vec{B}}{\left&space;|&space;\vec{B}&space;\right&space;|}&space;$$" target="_blank"><img src="https://latex.codecogs.com/png.latex?$$&space;\hat{B}&space;=&space;\frac{\vec{B}}{\left&space;|&space;\vec{B}&space;\right&space;|}&space;$$" title="$$ \hat{B} = \frac{\vec{B}}{\left | \vec{B} \right |} $$" /></a><br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$&space;\vec{D}&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})}{\left&space;|&space;\vec{B}&space;\right&space;|}&space;\ast&space;\frac{\vec{B}}{\left&space;|&space;\vec{B}&space;\right&space;|}&space;$$" target="_blank"><img src="https://latex.codecogs.com/png.latex?$$&space;\vec{D}&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})}{\left&space;|&space;\vec{B}&space;\right&space;|}&space;\ast&space;\frac{\vec{B}}{\left&space;|&space;\vec{B}&space;\right&space;|}&space;$$" title="$$ \vec{D} = \frac{(\vec{A} \cdot \vec{B})}{\left | \vec{B} \right |} \ast \frac{\vec{B}}{\left | \vec{B} \right |} $$" /></a><br><br>
因此最後獲得的公式<br>
<a href="https://www.codecogs.com/eqnedit.php?latex=$$&space;\vec{D}&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})\vec{B}}{\left&space;|&space;\vec{B}&space;\right&space;|^2}&space;$$" target="_blank"><img src="https://latex.codecogs.com/png.latex?$$&space;\vec{D}&space;=&space;\frac{(\vec{A}&space;\cdot&space;\vec{B})\vec{B}}{\left&space;|&space;\vec{B}&space;\right&space;|^2}&space;$$" title="$$ \vec{D} = \frac{(\vec{A} \cdot \vec{B})\vec{B}}{\left | \vec{B} \right |^2} $$" /></a>
<br>
<br>

在這的最後面加上了`m_camera.transform.position`的位置，因為最後求得的向量D在計算的時候都是以(0, 0, 0)去計算，所以最終得到的向量必須再加上攝影機位置才是我們真正要的向量D
```C#
void CalVectorD(int v_index)
{
    m_v3VecA[v_index] = m_sttObjPointData[v_index].point - m_camera.transform.position;
    m_v3VecB[v_index] = m_v3CenterVec - m_camera.transform.position;
    float fUnitVec = (Vector3.Dot(m_v3VecA[v_index], m_v3VecB[v_index]) / Common.DisForVector3(m_v3VecB[v_index]));
    m_v3VecD[v_index] = (m_v3VecB[v_index] * fUnitVec) + m_camera.transform.position;
}
```
<br>

### 相似形特性求長度C
<p align="left">
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/Artboard_6.png">
</p>
<p align="center"><em>圖 1-6. 相似形特性</em></p>

<br>

截至目前為止，灰、紫、紅、黃四個點的向量我們都有了，因此使用Unity內建的數學函示Vector3.Distance(a, b)來獲得圖中的A、B、D長度，
又因相似形特性<a href="https://www.codecogs.com/eqnedit.php?latex=\bar{A}&space;:&space;\bar{D}&space;=&space;\bar{C}&space;:&space;\bar{B}" target="_blank"><img src="https://latex.codecogs.com/png.latex?\bar{A}&space;:&space;\bar{D}&space;=&space;\bar{C}&space;:&space;\bar{B}" title="\bar{A} : \bar{D} = \bar{C} : \bar{B}" /></a>，所以 <a href="https://www.codecogs.com/eqnedit.php?latex=\bar{C}&space;=&space;(\bar{A}&space;\ast&space;\bar{B})&space;\div&space;\bar{D}" target="_blank"><img src="https://latex.codecogs.com/png.latex?\bar{C}&space;=&space;(\bar{A}&space;\ast&space;\bar{B})&space;\div&space;\bar{D}" title="\bar{C} = (\bar{A} \ast \bar{B}) \div \bar{D}" /></a>，因而求的C的長度

```C#
void CalLengthVecC(int v_index)
{
    m_fDisD[v_index] = Vector3.Distance(m_camera.transform.position, m_v3VecD[v_index]);
    m_fDisA[v_index] = Vector3.Distance(m_camera.transform.position, m_sttObjPointData[v_index].point);
    m_fDisC[v_index] = (m_fDisB * m_fDisA[v_index]) / m_fDisD[v_index];
}
```

