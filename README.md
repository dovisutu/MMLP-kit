# MMLP-kit
> 该仓库正在施工中。ETA: None
> 
> 以下的内容仅供参考，实际以施工完成后的readme为准。
> 
> 施工完成后，预计该仓库将移动至CFPAOrg下。
## 仓库简介
该仓库是用于[Minecraft-Mod-Language-Package](https://github.com/CFPAOrg/Minecraft-Mod-Language-Package)（MMLP）的检索库与Action集。

该仓库存放了以下内容：
* 用于检索MMLP仓库中语言文件的C#代码库。
* 与MMLP仓库流程相关的若干Action：
  * `pack-resource`，用于从MMLP仓库构建成型的Minecraft资源包。
  * `publish-resource-pack`，将构建的资源包发布到资源分发服务器上。
## 检索库 <!--呃，可能会单独放一个文件夹-->
### 为什么需要检索库？
因为MMLP在其文件结构中使用了一批**标记文件**，以实现文件引用、文件自动生成等高级功能，为仓库译者提供便利。然而，由于这些标记文件使用了较为复杂的功能，对文件结构的检索不再适合交由查询方实现，而应由仓库管理方提供。

本库就是用于查询检索MMLP仓库的代码库。该库以提供了对`namespace`一级文件的检索方法、对各种文件结构化的表示，以及遍历全仓库时可用的辅助工具——更多信息参见检索库目录下的readme文件。

## Action集


~好吧，这写起来还是有点怪~
