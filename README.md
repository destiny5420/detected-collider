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

截至目前為止，灰、紫、紅、黃四個點的向量我們都有了，因此使用Unity內建的數學函示Vector3.Distance(a, b)來獲得圖中的A、B、D長度，又因相似形特性<a href="https://www.codecogs.com/eqnedit.php?latex=\bar{A}&space;:&space;\bar{D}&space;=&space;\bar{C}&space;:&space;\bar{B}" target="_blank"><img src="https://latex.codecogs.com/png.latex?\bar{A}&space;:&space;\bar{D}&space;=&space;\bar{C}&space;:&space;\bar{B}" title="\bar{A} : \bar{D} = \bar{C} : \bar{B}" /></a>，所以<a href="https://www.codecogs.com/eqnedit.php?latex=\bar{C}&space;=&space;(\bar{A}&space;\ast&space;\bar{B})&space;\div&space;\bar{D}" target="_blank"><img src="https://latex.codecogs.com/png.latex?\bar{C}&space;=&space;(\bar{A}&space;\ast&space;\bar{B})&space;\div&space;\bar{D}" title="\bar{C} = (\bar{A} \ast \bar{B}) \div \bar{D}" /></a>，因而求得C的長度

```C#
void CalLengthVecC(int v_index)
{
    m_fDisD[v_index] = Vector3.Distance(m_camera.transform.position, m_v3VecD[v_index]);
    m_fDisA[v_index] = Vector3.Distance(m_camera.transform.position, m_sttObjPointData[v_index].point);
    m_fDisC[v_index] = (m_fDisB * m_fDisA[v_index]) / m_fDisD[v_index];
}
```
<br>
<br>

又因為<a href="https://www.codecogs.com/eqnedit.php?latex=\vec{C}" target="_blank"><img src="https://latex.codecogs.com/png.latex?\vec{C}" title="\vec{C}" /></a>與<a href="https://www.codecogs.com/eqnedit.php?latex=\vec{A}" target="_blank"><img src="https://latex.codecogs.com/png.latex?\vec{A}" title="\vec{A}" /></a>屬於同樣的方向，所以<a href="https://www.codecogs.com/eqnedit.php?latex=\vec{C}&space;=&space;\left&space;|&space;\vec{c}&space;\right&space;|&space;\hat{A}" target="_blank"><img src="https://latex.codecogs.com/png.latex?\vec{C}&space;=&space;\left&space;|&space;\vec{c}&space;\right&space;|&space;\hat{A}" title="\vec{C} = \left | \vec{c} \right | \hat{A}" /></a>

```C#
void CalResultPos(int v_index)
{
    Vector3 v3UnitVecA = Vector3.Normalize(m_v3VecA[v_index]);
    m_v3VecC[v_index] = v3UnitVecA * m_fLengthC[v_index];
    m_v3ResultPos[v_index] = m_v3VecC[v_index] + m_camera.transform.position; 
    m_v2ResultPos[v_index] = new Vector2(m_v3ResultPos[v_index].x, m_v3ResultPos[v_index].z);
}
```
最後一樣要將 ***向量加上camera的位置*** 才是我們最後要的藍點位置喔!
> 因最後只用兩軸來計算SAT，所以最後從Vec3轉換為Vec2<br>

<br>
<br>

到目前為止求得的四個點再依照SAT檢測就可以得到碰撞結果了。<br>
不過因為網路上已有作者針對SAT檢測有更詳細的文章解釋，所以可以[參考](http://davidhsu666.com/archives/gamecollisiondetection/)此作者的文章

<br>

## 總結
至目前為止已經解決了一開始想要做的部分，但還有些想到的問題尚未解決<br>
<br>

#### 1. 針對物件的旋轉，映射在虛擬平面上的點做相對應的旋轉
目前只針對Y軸有做此功能，如物件旋轉X或Z軸尚未支援，將會在未來有閒暇之餘補足這部分功能。<br>
<p>
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/GifImage_1.gif">
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/GifImage_2.gif">
</p>

#### 2. 碰撞偵測優化
目前的實驗Project只有兩個物件，如果將物件增加至千個，效能上肯定有非常大的落差，關於這點在SAT的作者有用了四叉樹(QuadTree)來做檢測優化，有興趣的讀者可以參考這篇[文章](http://davidhsu666.com/archives/quadtree_in_2d/)<br>
<br>
<br>

最後因眾多了考量，專案上還是沒有使用此機制來實作物件的碰撞，不過在研究的過程中卻是十分有趣的，此篇文章希望幫助其他讀者能就此延續更多有創意的想法，製作出更多有趣的功能。

</p>
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/GifImage_3.gif">
<img style="margin:auto;"  src="https://github.com/destiny5420/DetectedCollider/blob/SAT_Detected/GithubImage/GifImage_4.gif">
</p>