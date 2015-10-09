#region Copyright ?2007 Rotem Sapir
/*
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from the
 * use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not claim
 * that you wrote the original software. If you use this software in a product,
 * an acknowledgment in the product documentation is required, as shown here:
 *
 * Portions Copyright ?2007 Rotem Sapir
 *
 * 2. No substantial portion of the source code of this library may be redistributed
 * without the express written permission of the copyright holders, where
 * "substantial" is defined as enough code to be recognizably from this library.
*/
#endregion

namespace X3Platform.Membership.OrganizationUnitCharts
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Xml;
    using System.Linq;

    /// <summary>组织结构视图</summary>
    public class OrganizationUnitChartView : IDisposable
    {
        /// <summary>绘图类</summary>
        private Graphics graphics;
        /// <summary>图片文件宽度</summary>
        private int imageWidth = 0;
        /// <summary>图片文件高度</summary>
        private int imageHeight = 0;

        /// <summary>组织树形视图Xml文档</summary>
        private XmlDocument organizationTreeView;

        /// <summary>the employee data</summary>
        private IList<OrganizationUnitChartNode> cacheOrganizationUnitChartNodes;

        // private List<Position> EmpPositions;
        // A Dictionary of all employee data
        private SortedDictionary<string, OrganizationUnitChartNode> m_ChartNodes;

        /// <summary>A list of the chart node positions.</summary>
        public SortedDictionary<string, OrganizationUnitChartNode> ChartNodes
        {
            get { return m_ChartNodes; }
            set { m_ChartNodes = value; }
        }

        #region 属性:BoxFillColor
        private Color m_BoxBackgroundColor = Color.White;

        /// <summary>长方形背景色</summary>
        public Color BoxBackgroundColor
        {
            get { return m_BoxBackgroundColor; }
            set { m_BoxBackgroundColor = value; }
        }
        #endregion

        #region 属性:BoxWidth
        private int m_BoxWidth = 120;

        /// <summary>长方型宽度</summary>
        public int BoxWidth
        {
            get { return m_BoxWidth; }
            set { m_BoxWidth = value; }
        }
        #endregion

        #region 属性:BoxHeight
        private int m_BoxHeight = 48;

        /// <summary>长方型高度</summary>
        public int BoxHeight
        {
            get { return m_BoxHeight; }
            set { m_BoxHeight = value; }
        }
        #endregion

        #region 属性:Margin
        private int m_Margin = 10;

        /// <summary>留白</summary>
        public int Margin
        {
            get { return m_Margin; }
            set { m_Margin = value; }
        }
        #endregion

        #region 属性:HorizontalSpace
        private int m_HorizontalSpace = 30;

        /// <summary>垂直空间</summary>
        public int HorizontalSpace
        {
            get { return m_HorizontalSpace; }
            set { m_HorizontalSpace = value; }
        }
        #endregion

        #region 属性:VerticalSpace
        private int m_VerticalSpace = 30;

        /// <summary>垂直空间</summary>
        public int VerticalSpace
        {
            get { return m_VerticalSpace; }
            set { m_VerticalSpace = value; }
        }
        #endregion

        #region 属性:FontSize
        private float m_FontSize = 9.0F;

        /// <summary>字体大小</summary>
        public float FontSize
        {
            get { return m_FontSize; }
            set { m_FontSize = value; }
        }
        #endregion

        #region 属性:LineColor
        private Color _LineColor = Color.Black;

        /// <summary></summary>
        public Color LineColor
        {
            get { return _LineColor; }
            set { _LineColor = value; }
        }
        #endregion

        #region 属性:LineWidth
        private float m_LineWidth = 1;

        /// <summary></summary>
        public float LineWidth
        {
            get { return m_LineWidth; }
            set { m_LineWidth = value; }
        }
        #endregion

        private Color m_BackgroundColor = Color.White;

        /// <summary>背景色</summary>
        public Color BackgroundColor
        {
            get { return m_BackgroundColor; }
            set { m_BackgroundColor = value; }
        }

        private Color _FontColor = Color.Black;
        
        /// <summary></summary>
        public Color FontColor
        {
            get { return _FontColor; }
            set { _FontColor = value; }
        }

        /// <summary></summary>
        public OrganizationUnitChartView(IList<OrganizationUnitChartNode> list)
        {
            this.cacheOrganizationUnitChartNodes = list;
        }

        /// <summary></summary>
        public void Dispose()
        {
            this.cacheOrganizationUnitChartNodes = null;

            if (this.graphics != null)
            {
                this.graphics.Dispose();
                this.graphics = null;
            }
        }

        /// <summary>The main function used to create the image</summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rootId">The ID of the Boss from which to start building the chart</param>
        /// <param name="imageFormat"></param>
        /// <returns>A stream of the image. can be shown or saved to disk.</returns>
        public Stream GenerateOrganizationUnitChart(int width, int height, string rootId, ImageFormat imageFormat)
        {
            MemoryStream result = new MemoryStream();

            this.ChartNodes = new SortedDictionary<string, OrganizationUnitChartNode>();

            //reset image size
            imageHeight = 0;
            imageWidth = 0;

            //define the image
            this.organizationTreeView = null;

            this.organizationTreeView = new XmlDocument();

            XmlNode root = this.organizationTreeView.CreateNode(XmlNodeType.Element, "root", "");

            XmlAttribute attribute = this.organizationTreeView.CreateAttribute("id");

            attribute.InnerText = rootId;

            root.Attributes.Append(attribute);

            this.organizationTreeView.AppendChild(root);

            BuildTree(root, 0);

            //uncomment lines below to save the xml created, for debugging.
            //XmlTextWriter xw = new XmlTextWriter(@"d:\temp\1.xml", Encoding.UTF8);
            //this.organizationTreeView.WriteTo(xw);
            //xw.Flush();
            //xw.Close();

            using (Bitmap bitmap = new Bitmap(imageWidth, imageHeight))
            {
                this.graphics = Graphics.FromImage(bitmap);

                this.graphics.Clear(this.BackgroundColor);

                DrawOrganizationUnitChart(root);

                //if caller does not care about size, use original calculated size
                if (width < 0)
                {
                    width = imageWidth;
                }

                if (height < 0)
                {
                    height = imageHeight;
                }

                // 重新计算图片位置
                using (Bitmap resizedBitmap = new Bitmap(bitmap, new Size(width, height)))
                {
                    //after resize - change the coordinates of the list, in order return the proper coordinates
                    //for each employee
                    CalculateImageMapData(width, height);

                    resizedBitmap.Save(result, imageFormat);
                    resizedBitmap.Dispose();
                }

                bitmap.Dispose();
            }

            this.graphics.Dispose();

            return result;
        }

        /// <summary>
        /// This overloaded method can be used to return the image using it's default calculated size, without resizing
        /// </summary>
        /// <param name="rootNodeId"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        public Stream GenerateOrganizationUnitChart(string rootNodeId, ImageFormat imageFormat)
        {
            return GenerateOrganizationUnitChart(-1, -1, rootNodeId, imageFormat);
        }

        private void BuildTree(XmlNode node, int y)
        {
            IList<OrganizationUnitChartNode> list = this.cacheOrganizationUnitChartNodes.Where(
                chartNode => chartNode.ParentId == node.Attributes["id"].Value).ToList();

            //has employees
            foreach (OrganizationUnitChartNode item in list)
            {
                //for each employee call this function again
                XmlNode NewEmployeeNode = this.organizationTreeView.CreateElement("node");

                //NewEmployeeNode.InnerText = EmployeeRow.id;
                XmlAttribute attribute = this.organizationTreeView.CreateAttribute("id");

                attribute.InnerText = item.Id;

                NewEmployeeNode.Attributes.Append(attribute);

                node.AppendChild(NewEmployeeNode);

                BuildTree(NewEmployeeNode, y + 1);
            }

            //build employee data
            //after checking for employees we can add the current employee
            OrganizationUnitChartNode organizationObject = this.cacheOrganizationUnitChartNodes.Where(
                chartNode => chartNode.Id == node.Attributes["id"].Value).First();

            organizationObject.ChildNodeCount = node.ChildNodes.Count;

            int startX;
            int startY;

            int[] ResultsArr = new int[] {GetXPositionByOwnChildren(node),
                                    GetXPositionByParentPreviousSibling(node),
                                    GetXPositionByPreviousSibling(node),
                                    this.Margin };
            Array.Sort(ResultsArr);

            startX = ResultsArr[3];

            startY = (y * (this.BoxHeight + this.VerticalSpace)) + this.Margin;

            int width = this.BoxWidth;
            int height = this.BoxHeight;

            //update the coordinates of this box into the matrix, for later calculations

            Rectangle drawRectangle = new Rectangle(startX, startY, width, height);

            //update the image size
            if (imageWidth < (startX + width + this.Margin))
            {
                imageWidth = startX + width + this.Margin;
            }

            if (imageHeight < (startY + height + this.Margin))
            {
                imageHeight = startY + height + this.Margin;
            }

            organizationObject.Position = drawRectangle;

            this.ChartNodes.Add(organizationObject.Id, organizationObject);

            organizationObject.Dispose();

            organizationObject = null;
        }

        /************************************************************************************************************************
         * The box position is affected by:
         * 1. The previous sibling (box on the same level)
         * 2. The positions of it's children
         * 3. The position of it's uncle (parents' previous sibling)/ cousins (parents' previous sibling children)
         * What determines the position is the farthest x of all the above. If all/some of the above have no value, the margin 
         * becomes the dtermining factor.
         * **********************************************************************************************************************
        */

        private int GetXPositionByOwnChildren(XmlNode node)
        {
            int result = -1;

            if (node.HasChildNodes)
            {
                result = (((this.ChartNodes[node.LastChild.Attributes["id"].Value].Position.X + this.BoxWidth) -
                    this.ChartNodes[node.FirstChild.Attributes["id"].Value].Position.X) / 2) -
                    (this.BoxWidth / 2) +
                    this.ChartNodes[node.FirstChild.Attributes["id"].Value].Position.X;
            }

            return result;
        }

        private int GetXPositionByPreviousSibling(XmlNode node)
        {
            int result = -1;

            XmlNode previousSibling = node.PreviousSibling;

            if (previousSibling != null)
            {
                if (previousSibling.HasChildNodes)
                {
                    result = this.ChartNodes[previousSibling.LastChild.Attributes["id"].Value].Position.X + this.BoxWidth + this.HorizontalSpace;
                }
                else
                {
                    result = this.ChartNodes[previousSibling.Attributes["id"].Value].Position.X + this.BoxWidth + this.HorizontalSpace;
                }
            }

            return result;
        }

        private int GetXPositionByParentPreviousSibling(XmlNode node)
        {
            int result = -1;

            XmlNode parentPreviousSibling = node.ParentNode.PreviousSibling;

            if (parentPreviousSibling == null)
            {
                if (node.ParentNode.Name != "root" && node.ParentNode.Name != "#document")
                {
                    result = GetXPositionByParentPreviousSibling(node.ParentNode);
                }
            }
            else
            {
                if (parentPreviousSibling.HasChildNodes)
                {
                    result = this.ChartNodes[parentPreviousSibling.LastChild.Attributes["id"].Value].Position.X + this.BoxWidth + this.HorizontalSpace;
                }
                else
                {
                    result = this.ChartNodes[parentPreviousSibling.Attributes["id"].Value].Position.X + this.BoxWidth + this.HorizontalSpace;
                }
            }

            return result;
        }

        /// <summary>Draws the actual chart image.</summary>
        private void DrawOrganizationUnitChart(XmlNode node)
        {
            // 
            // 字体 和 笔刷
            //

            // 字体
            Font font = new Font("微软雅黑", this.FontSize); // Verdana | Microsoft YaHei Font
            // 笔刷
            SolidBrush brush = new SolidBrush(this.FontColor);
            // 钢笔
            Pen pen = new Pen(this.LineColor, this.LineWidth);
            // 字符格式
            StringFormat format = new StringFormat();
            // 垂直对齐
            format.Alignment = StringAlignment.Center;
            // 水平对齐
            format.LineAlignment = StringAlignment.Center;
            // 竖着显示文字
            // format.FormatFlags = StringFormatFlags.DirectionVertical;

            IList<OrganizationUnitChartNode> list = cacheOrganizationUnitChartNodes.Where(charNode => charNode.ParentId == node.Attributes["id"].Value).ToList();

            foreach (OrganizationUnitChartNode chartNode in list)
            {
                DrawOrganizationUnitChart(node.SelectSingleNode(string.Format("node[@id='{0}']", chartNode.Id)));
            }

            this.graphics.DrawRectangle(new Pen(this.LineColor, this.LineWidth * 2),
                this.ChartNodes[node.Attributes["id"].Value].Position);

            this.graphics.FillRectangle(new SolidBrush(this.BoxBackgroundColor),
                this.ChartNodes[node.Attributes["id"].Value].Position);

            // 构建字符串
            string text = this.ChartNodes[node.Attributes["id"].Value].Name;

            if (string.IsNullOrEmpty(this.ChartNodes[node.Attributes["id"].Value].Code))
            {
                text = this.ChartNodes[node.Attributes["id"].Value].Code + Environment.NewLine + text;
            }

            if (string.IsNullOrEmpty(this.ChartNodes[node.Attributes["id"].Value].Description))
            {
                text = this.ChartNodes[node.Attributes["id"].Value].Description + Environment.NewLine + text;
            }

            // Draw string to screen.
            this.graphics.DrawString(text, font, brush, this.ChartNodes[node.Attributes["id"].Value].Position, format);

            //draw connecting lines

            if (node.Name != "root")
            {
                //all but the top box should have lines growing out of their top
                this.graphics.DrawLine(pen,
                    this.ChartNodes[node.Attributes["id"].Value].Position.Left + (this.BoxWidth / 2),
                    this.ChartNodes[node.Attributes["id"].Value].Position.Top,
                    this.ChartNodes[node.Attributes["id"].Value].Position.Left + (this.BoxWidth / 2),
                    this.ChartNodes[node.Attributes["id"].Value].Position.Top - (this.VerticalSpace / 2));
            }

            if (node.HasChildNodes)
            {
                //all employees which have employees should have lines coming from bottom down
                this.graphics.DrawLine(pen,
                    this.ChartNodes[node.Attributes["id"].Value].Position.Left + (this.BoxWidth / 2),
                    this.ChartNodes[node.Attributes["id"].Value].Position.Top + this.BoxHeight,
                    this.ChartNodes[node.Attributes["id"].Value].Position.Left + (this.BoxWidth / 2),
                    this.ChartNodes[node.Attributes["id"].Value].Position.Top + this.BoxHeight + (this.VerticalSpace / 2));
            }

            if (node.PreviousSibling != null)
            {
                //the prev employee has the same boss - connect the 2 employees
                this.graphics.DrawLine(pen,
                    this.ChartNodes[node.PreviousSibling.Attributes["id"].Value].Position.Left + (this.BoxWidth / 2) - (this.LineWidth / 2),
                    this.ChartNodes[node.PreviousSibling.Attributes["id"].Value].Position.Top - (this.VerticalSpace / 2),
                    this.ChartNodes[node.Attributes["id"].Value].Position.Left + (this.BoxWidth / 2) + (this.LineWidth / 2),
                    this.ChartNodes[node.Attributes["id"].Value].Position.Top - (this.VerticalSpace / 2));
            }
        }

        /// <summary>计算图片</summary>
        /// <param name="actualWidth"></param>
        /// <param name="actualHeight"></param>
        private void CalculateImageMapData(double actualWidth, double actualHeight)
        {
            double percentageChangeX = actualWidth / imageWidth;
            double percentageChangeY = actualHeight / imageHeight;

            foreach (OrganizationUnitChartNode item in this.ChartNodes.Values)
            {
                Rectangle resizedRectangle = new Rectangle(
                    (int)(item.Position.X * percentageChangeX),
                    (int)(item.Position.Y * percentageChangeY),
                    (int)(this.BoxWidth * percentageChangeX),
                    (int)(this.BoxHeight * percentageChangeY));

                item.Position = resizedRectangle;
            }
        }
    }
}
