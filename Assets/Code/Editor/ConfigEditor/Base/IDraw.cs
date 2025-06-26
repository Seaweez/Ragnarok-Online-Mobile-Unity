namespace EditorTool
{
	/// <summary>
	/// 实现这个接口的类能绘制左侧列表内的标题, 以及右侧的详情
	/// </summary>
	public interface IDraw
    {
        //绘制左侧列表内的标题，包含ID和Name
        void DrawTitle(int firstRow, int totalRowNum);

        //在详情面板ConfigInSpectorWindow上绘制配置详情
        void DrawDetail();

        int rownum { get; set; }
        /// <summary>
        /// 设置每条配置对应的行号，供布局时使用
        /// </summary>
        /// <param name="lastIdx">当前最后一行的行号</param>
        /// <returns>设置完成后最后一行的行号</returns>
        int SetRowNum(int lastIdx);
        /// <summary>
        /// 设置子配置的行号
        /// </summary>
        /// <param name="lastIdx"></param>
        /// <returns></returns>
        int SetChildRowNum(int lastIdx);
		IDraw GetChild (int row);
    }
}