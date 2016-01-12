using System;
using System.Collections;
using Xamarin.Forms;
using System.Diagnostics;
using TwinTechs.Controls;

namespace XLabs.Forms.Controls
{
	public interface IGridViewProvider
	{
		void ReloadData ();

		void ScrollToItemWithIndex (int index, bool animated);
	}

	/// <summary>
	/// Class GridView.
	/// </summary>
	public class GridView2 : ContentView, IScrollAwareElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GridView"/> class.
		/// </summary>
		public GridView2 ()
		{
			SelectionEnabled = true;
		}


		//
		// Static Fields
		//
		/// <summary>
		/// The items source property
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create ("ItemsSource", typeof(IEnumerable), typeof(GridView2), null, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The item template property
		/// </summary>
		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create ("ItemTemplate", typeof(DataTemplate), typeof(GridView2), null, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The row spacing property
		/// </summary>
		public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create ("RowSpacing", typeof(double), typeof(GridView2), (double)0, BindingMode.OneWay, null, null, null, null);

		public static readonly BindableProperty MaxItemsPerRowProperty = BindableProperty.Create ("MaxItemsPerRow", typeof(int), typeof(GridView2), -1, BindingMode.OneWay, null, null, null, null);
		/// <summary>
		/// The row spacing property
		/// </summary>
		public static readonly BindableProperty CenterAsFilledRowProperty = BindableProperty.Create ("CenterAsFilledRow", typeof(bool), typeof(GridView2), true, BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty IsScrollEnabledProperty = BindableProperty.Create ("IsScrollEnabled", typeof(bool), typeof(GridView2), true, BindingMode.OneWay, null, null, null, null);


		/// <summary>
		/// The column spacing property
		/// </summary>
		public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create ("ColumnSpacing", typeof(double), typeof(GridView2), (double)0, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The item width property
		/// </summary>
		public static readonly BindableProperty ItemWidthProperty = BindableProperty.Create ("ItemWidth", typeof(double), typeof(GridView2), (double)100, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The item height property
		/// </summary>
		public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create ("ItemHeight", typeof(double), typeof(GridView2), (double)100, BindingMode.OneWay, null, null, null, null);


		//TODO move these into a rectangle

		/// <summary>
		/// Paddingleft
		/// </summary>
		public static readonly BindableProperty ContentPaddingLeftProperty = BindableProperty.Create ("ContentPaddingLeft", typeof(double), typeof(GridView2), (double)0, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// Paddingright
		/// </summary>
		public static readonly BindableProperty ContentPaddingRightProperty = BindableProperty.Create ("ContentPaddingRight", typeof(double), typeof(GridView2), (double)0, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// PaddingBottom
		/// </summary>
		public static readonly BindableProperty ContentPaddingBottomProperty = BindableProperty.Create ("ContentPaddingBottom", typeof(double), typeof(GridView2), (double)0, BindingMode.OneWay, null, null, null, null);

		public static readonly BindableProperty SectionPaddingBottomProperty = BindableProperty.Create ("SectionPaddingBottom", typeof(double), typeof(GridView2), (double)0, BindingMode.OneWay, null, null, null, null);

		public static readonly BindableProperty SectionPaddingTopProperty = BindableProperty.Create ("SectionPaddingTop", typeof(double), typeof(GridView2), (double)0, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// Paddingtop
		/// </summary>
		public static readonly BindableProperty ContentPaddingTopProperty = BindableProperty.Create ("ContentPaddingTop", typeof(double), typeof(GridView2), (double)0, BindingMode.OneWay, null, null, null, null);

		public static readonly BindableProperty IsHorizontalProperty = BindableProperty.Create ("IsHorizontal", typeof(bool), typeof(GridView2), false, BindingMode.OneWay, null, null, null, null);

		public static readonly BindableProperty ItemSizeProperty = BindableProperty.Create ("ItemSize", typeof(Size), typeof(GridView2), new Size (100, 100), BindingMode.OneWay, null, null, null, null);

		public static readonly BindableProperty IsContentCenteredProperty = BindableProperty.Create<GridView2,bool> (s => s.IsContentCentered, true);

		public bool IsContentCentered {
			get{ return (bool)GetValue (IsContentCenteredProperty); }
			set{ SetValue (IsContentCenteredProperty, value); }
		}

		int? _initialIndex;

		IGridViewProvider _gridViewProvider;

		public IGridViewProvider GridViewProvider {
			get{ return _gridViewProvider; }
			set {
				_gridViewProvider = value;
				if (_initialIndex.HasValue) {
					GridViewProvider.ScrollToItemWithIndex (_initialIndex.Value, false);
					_initialIndex = null;
				}
			}
		}

		//
		// Properties
		//
		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public IEnumerable ItemsSource {
			get {
				return (IEnumerable)base.GetValue (GridView2.ItemsSourceProperty);
			}
			set {
				base.SetValue (GridView2.ItemsSourceProperty, value);
			}
		}

		public Size ItemSize {
			get {
				return (Size)base.GetValue (GridView2.ItemSizeProperty);
			}
			set {
				base.SetValue (GridView2.ItemSizeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the item template.
		/// </summary>
		/// <value>The item template.</value>
		public DataTemplate ItemTemplate {
			get {
				return (DataTemplate)base.GetValue (GridView2.ItemTemplateProperty);
			}
			set {
				base.SetValue (GridView2.ItemTemplateProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the row spacing.
		/// </summary>
		/// <value>The row spacing.</value>
		public double RowSpacing {
			get {
				return (double)base.GetValue (GridView2.RowSpacingProperty);
			}
			set {
				base.SetValue (GridView2.RowSpacingProperty, value);
			}
		}

		public int MaxItemsPerRow {
			get {
				return (int)base.GetValue (GridView2.MaxItemsPerRowProperty);
			}
			set {
				base.SetValue (GridView2.MaxItemsPerRowProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the column spacing.
		/// </summary>
		/// <value>The column spacing.</value>
		public double ColumnSpacing {
			get {
				return (double)base.GetValue (GridView2.ColumnSpacingProperty);
			}
			set {
				base.SetValue (GridView2.ColumnSpacingProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the column spacing.
		/// </summary>
		/// <value>The column spacing.</value>
		public bool CenterAsFilledRow {
			get {
				return (bool)base.GetValue (GridView2.CenterAsFilledRowProperty);
			}
			set {
				base.SetValue (GridView2.CenterAsFilledRowProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is scroll enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is scroll enabled; otherwise, <c>false</c>.</value>
		public bool IsScrollEnabled {
			get {
				return (bool)base.GetValue (GridView2.IsScrollEnabledProperty);
			}
			set {
				base.SetValue (GridView2.IsScrollEnabledProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the width of the item.
		/// </summary>
		/// <value>The width of the item.</value>
		public double ItemWidth {
			get {
				return (double)base.GetValue (GridView2.ItemWidthProperty);
			}
			set {
				base.SetValue (GridView2.ItemWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the height of the item.
		/// </summary>
		/// <value>The height of the item.</value>
		public double ItemHeight {
			get {
				return (double)base.GetValue (GridView2.ItemHeightProperty);
			}
			set {
				base.SetValue (GridView2.ItemHeightProperty, value);
			}
		}

		//TODO move into a rect


		/// <summary>
		/// Gets or sets the ContentPaddingLeft of the item.
		/// </summary>
		/// <value>The ContentPaddingLeft of the item.</value>
		public double ContentPaddingLeft {
			get {
				return (double)base.GetValue (GridView2.ContentPaddingLeftProperty);
			}
			set {
				base.SetValue (GridView2.ContentPaddingLeftProperty, value);
			}
		}


		/// <summary>
		/// Gets or sets the ContentPaddingRight of the item.
		/// </summary>
		/// <value>The ContentPaddingRight of the item.</value>
		public double ContentPaddingRight {
			get {
				return (double)base.GetValue (GridView2.ContentPaddingRightProperty);
			}
			set {
				base.SetValue (GridView2.ContentPaddingRightProperty, value);
			}
		}



		/// <summary>
		/// Gets or sets the ContentPaddingTop of the item.
		/// </summary>
		/// <value>The ContentPaddingTop of the item.</value>
		public double ContentPaddingTop {
			get {
				return (double)base.GetValue (GridView2.ContentPaddingTopProperty);
			}
			set {
				base.SetValue (GridView2.ContentPaddingTopProperty, value);
			}
		}



		/// <summary>
		/// Gets or sets the ContentPaddingBottom of the item.
		/// </summary>
		/// <value>The ContentPaddingBottom of the item.</value>
		public double ContentPaddingBottom {
			get {
				return (double)base.GetValue (GridView2.ContentPaddingBottomProperty);
			}
			set {
				base.SetValue (GridView2.ContentPaddingBottomProperty, value);
			}
		}

		public double SectionPaddingTop {
			get {
				return (double)base.GetValue (GridView2.SectionPaddingTopProperty);
			}
			set {
				base.SetValue (GridView2.SectionPaddingTopProperty, value);
			}
		}

		public double SectionPaddingBottom {
			get {
				return (double)base.GetValue (GridView2.SectionPaddingBottomProperty);
			}
			set {
				base.SetValue (GridView2.SectionPaddingBottomProperty, value);
			}
		}

		/// <summary>
		/// Occurs when item is selected.
		/// </summary>
		public event EventHandler<GridEventArgs<object>> ItemSelected;

		/// <summary>
		/// Invokes the item selected event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="item">Item.</param>
		public void InvokeItemSelectedEvent (object sender, object item)
		{
			if (this.ItemSelected != null) {
				this.ItemSelected.Invoke (sender, new GridEventArgs<object> (item));
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [selection enabled].
		/// </summary>
		/// <value><c>true</c> if [selection enabled]; otherwise, <c>false</c>.</value>
		public bool SelectionEnabled {
			get;
			set;
		}

		public bool IsHorizontal {
			get {
				return (bool)base.GetValue (GridView2.IsHorizontalProperty);
			}
			set {
				base.SetValue (GridView2.IsHorizontalProperty, value);
			}
		}

		public void ReloadData ()
		{
			if (GridViewProvider != null) {
				GridViewProvider.ReloadData ();
			}
		}


		public void ScrollToItemWithIndex (int index, bool animated)
		{
			if (GridViewProvider != null) {
				GridViewProvider.ScrollToItemWithIndex (index, animated);
			} else {
				_initialIndex = index;
			}
		}

		#region ISCrollAwareElement

		public event EventHandler OnStartScroll;
		public event EventHandler OnStopScroll;
		public event EventHandler<ControlScrollEventArgs> OnScroll;

		public void RaiseOnScroll (float delta, float currentY, int currentRow)
		{
			var args = new ControlScrollEventArgs (delta, currentY, currentRow);
			if (OnScroll != null) {
				OnScroll (this, args);
			}
		}

		public void RaiseOnStartScroll ()
		{
			if (OnStartScroll != null) {
				OnStartScroll (this, new EventArgs ());
			}
		}

		public void RaiseOnStopScroll ()
		{
			if (OnStopScroll != null) {
				OnStopScroll (this, new EventArgs ());
			}
		}

		#endregion
	}
}
