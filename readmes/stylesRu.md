## Если хотите написать свой стиль, вы можете создать папку(если она не создана) Styles/ в корневом каталоге приложения. В нем создайте файл стиля. Затем, в окне настроек появится созданный стиль.
- ## [Примеры стилей](https://github.com/OOjeser/DesktopTiles/tree/master/AvaloniaApplication1/Styles). 
- ## [Дополнительная информация о создании стилей](https://docs.avaloniaui.net/docs/styling).
### [Разметка панели кнопок (может быть полезно)](https://github.com/OOjeser/DesktopTiles/blob/master/AvaloniaApplication1/Views/MainWindow.axaml):
    <DataTemplate DataType="models:TileItem">
        <Button Classes="Tile ItemTile" Click="Button_OnClick" CommandParameter="{Binding}">
            <Border Classes="ItemTileBorder">
                <Grid Classes="ItemTileContainer">
                    <Image Classes="ItemTileImage"/>
                    <TextBlock Classes="ItemTileTitle"/>
                </Grid>
            </Border>
            <Button.ContextMenu>
                <ContextMenu>
                    <MenuItem Header="Изменить иконку"/>
                </ContextMenu>
            </Button.ContextMenu>
        </Button>
    </DataTemplate>
    <DataTemplate DataType="models:TileFolder">
        <Button Classes="Tile FolderTile" Click="Button_OnClick" CommandParameter="{Binding}">
            <Border Classes="FolderTileBorder">
                <Grid Classes="FolderTileContainer">
                    <Grid Classes="FolderTileImagesContainer" ColumnDefinitions="* *" RowDefinitions="* *">
                        <Image Classes="FolderTileImage" Grid.Row="0" Grid.Column="0" Source="{Binding Icon1}"/>
                        <Image Classes="FolderTileImage" Grid.Row="1" Grid.Column="0" Source="{Binding Icon2}"/>
                        <Image Classes="FolderTileImage" Grid.Row="0" Grid.Column="1" Source="{Binding Icon3}"/>
                        <Image Classes="FolderTileImage" Grid.Row="1" Grid.Column="1" Source="{Binding Icon4}"/>
                    </Grid>
                    <TextBlock Classes="FolderTileTitle"/>
                </Grid>
            </Border>
        </Button>
    </DataTemplate>
### [Пример стиля](https://github.com/OOjeser/DesktopTiles/blob/master/AvaloniaApplication1/Styles/AndroidStyle.axaml)
    <Styles xmlns="https://github.com/avaloniaui"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
        <Style Selector="Button.Tile">
            <Setter Property="Transitions">
                <Setter.Value>
                    <Transitions>
                        <DoubleTransition Property="Opacity" Duration="0:0:0.1"/>
                        <DoubleTransition Property="Width" Duration="0:0:0.1"/>
                        <DoubleTransition Property="Height" Duration="0:0:0.1"/>
                        <ThicknessTransition Property="Margin" Duration="0:0:0.1"/>
                        <BrushTransition Property="Background" Duration="0:0:0.1"/>
                        <CornerRadiusTransition Property="CornerRadius" Duration="0:0:0.1"/>
                    </Transitions>
                </Setter.Value>
            </Setter>
            <Setter Property="Background" Value="{Binding MainColorBrush}"/>
            <Setter Property="Margin" Value="30, 20"/>
            <Setter Property="CornerRadius" Value="130"/>
            <Setter Property="ClipToBounds" Value="False"/>
        </Style>
        <Style Selector="TextBlock.ItemTileTitle">
            <Setter Property="Margin" Value="50, 0, 0, 0"/>
            <Setter Property="RenderTransform">
                <Setter.Value>
                    <TranslateTransform Y="25"/>
                </Setter.Value>
            </Setter>
        </Style>
        <Style Selector="TextBlock.FolderTileTitle">
            <Setter Property="RenderTransform">
                <Setter.Value>
                    <TranslateTransform Y="25"/>
                </Setter.Value>
            </Setter>
        </Style>
        <Style Selector="Button > Grid > Image">
            <Setter Property="Width" Value="50"/>
            <Setter Property="Height" Value="50"/>
            <Setter Property="Opacity" Value="1"/>
            <Setter Property="Transitions">
                <Setter.Value>
                    <Transitions>
                        <DoubleTransition Property="Width" Duration="0:0:0.1"/>
                        <DoubleTransition Property="Height" Duration="0:0:0.1"/>
                        <DoubleTransition Property="Opacity" Duration="0:0:0.1"/>
                    </Transitions>
                </Setter.Value>
            </Setter>
        </Style>
        <Style Selector="Button > Grid > Grid">
            <Setter Property="Width" Value="100"/>
            <Setter Property="Height" Value="100"/>
            <Setter Property="Transitions">
                <Setter.Value>
                    <Transitions>
                        <DoubleTransition Property="Width" Duration="0:0:0.1"/>
                        <DoubleTransition Property="Height" Duration="0:0:0.1"/>
                    </Transitions>
                </Setter.Value>
            </Setter>
        </Style>
        <Style Selector="Button > Grid > Grid > Image">
            <Setter Property="Width" Value="40"/>
            <Setter Property="Height" Value="40"/>
            <Setter Property="HorizontalAlignment" Value="Center"/>
            <Setter Property="VerticalAlignment" Value="Center"/>
            <Setter Property="Transitions">
                <Setter.Value>
                    <Transitions>
                        <DoubleTransition Property="Width" Duration="0:0:0.1"/>
                        <DoubleTransition Property="Height" Duration="0:0:0.1"/>
                    </Transitions>
                </Setter.Value>
            </Setter>
        </Style>
        <Style Selector="Button.ItemTile > Border">
            <Setter Property="CornerRadius" Value="130"/>
            <Setter Property="Transitions">
                <Setter.Value>
                    <Transitions>
                        <CornerRadiusTransition Property="CornerRadius" Duration="0:0:0.1"/>
                    </Transitions>
                </Setter.Value>
            </Setter>
        </Style>
            <Style Selector="Button.ItemTile:pointerover > Border">
                <Setter Property="CornerRadius" Value="110"/>
            <Setter Property="Background" Value="#2000"/>
        </Style>
        <Style Selector="Button:pointerover /template/ ContentPresenter#PART_ContentPresenter">
            <Setter Property="Background" Value="{Binding MainColorBrush}"/>
        </Style>
        <Style Selector="Button:pointerover > Grid > Image">
        </Style>
        <Style Selector="Button:pressed">
            <Setter Property="Background" Value="#e000"/>
        </Style>

        <Style Selector="Button.Tile">
            <Setter Property="Width" Value="280"/>
            <Setter Property="Height" Value="280"/>
            <Setter Property="Padding" Value="0"/>
        </Style>
        <Style Selector="Button.Tile:pointerover">
            <Setter Property="Width" Value="280"/>
            <Setter Property="Height" Value="280"/>
            <Setter Property="Padding" Value="0"/>
            <Setter Property="CornerRadius" Value="110"/>
        </Style>
        <Style Selector="Button.openAnimation">
        </Style>
        <Style Selector="Button.openAnimation:pointerover /template/ ContentPresenter#PART_ContentPresenter">
        </Style>
        <Style Selector="Button.openAnimation > Grid > Image">
            <Setter Property="Width" Value="500"/>
            <Setter Property="Height" Value="500"/>
            <Setter Property="Opacity" Value="0"/>
        </Style>
        <Style Selector="Button.BackButton">
            <Setter Property="CornerRadius" Value="0"/>
        </Style>
        <Style Selector="Button.BackButton /template/ ContentPresenter#PART_ContentPresenter">
            <Setter Property="Background" Value="#fff"/>
        </Style>
        <Style Selector="TextBlock.BackButtonText">
            <Setter Property="Background" Value="#0fff"/>
            <Setter Property="Foreground" Value="#fff"/>
        </Style>
        <Style Selector="TextBlock.BackButtonText:pointerover">
            <Setter Property="Foreground" Value="#eee"/>
        </Style>
    </Styles>
