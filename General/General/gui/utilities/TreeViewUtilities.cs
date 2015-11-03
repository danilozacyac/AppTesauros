using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace mx.gob.scjn.ius_common.gui.gui.utilities
{
    public class TreeViewUtilities
    {
        public static void ExpandeTodo(ItemsControl parentContainer)
        {
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    currentContainer.IsExpanded = true;
                    if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                    {
                        ExpandeTodo(currentContainer);
                    }
                    else
                    {
                        currentContainer.ItemContainerGenerator.StatusChanged += delegate
                        {
                            ExpandeTodo(currentContainer);
                        };
                    }
                }
            }
        }

        public static void ContraeTodo(ItemsControl parentContainer)
        {
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    currentContainer.IsExpanded = false;
                    if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                    {
                        ContraeTodo(currentContainer);
                    }
                    else
                    {
                        currentContainer.ItemContainerGenerator.StatusChanged += delegate
                        {
                            ContraeTodo(currentContainer);
                        };
                    }
                }
            }
        }


        public static void ExpandeConMensajes(ItemsControl arbol)
        {
            foreach (Object item in arbol.Items)
            {
                TreeViewItem currentContainer = arbol.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    currentContainer.IsExpanded = true;
                    Sheva.Windows.WpfApplication.DoEvents();
                    ExpandeConMensajes(currentContainer);
                }
            }
        }
        public static void SeleccionaNodo(int index, TreeView parentContainer)
        {
            TreeViewItem currentContainer = GetContainerFromPathEntity((String)((List<TreeViewItem>)parentContainer.ItemsSource).ElementAt(index).Header,
                parentContainer);
            if (currentContainer == null)
            {
                currentContainer = parentContainer.ItemContainerGenerator.ContainerFromIndex(index) as TreeViewItem;
                if (currentContainer != null)
                {
                    currentContainer.IsSelected = true;
                    currentContainer.BringIntoView();
                    currentContainer.Focus();
                    return;
                }
            }
            else
            {
                currentContainer.IsSelected = true;
                currentContainer.BringIntoView();
                currentContainer.Focus();
            }
        }
        public static void SeleccionaNodo(String[] pathEntities,
            Int32 currentIndex,
            ItemsControl parentContainer)
        {
            if (currentIndex >= pathEntities.Length) return;

            TreeViewItem currentContainer = GetContainerFromPathEntity(pathEntities[currentIndex], parentContainer);
            if (currentContainer == null)
            {
                if (currentIndex == 0)
                {
                    currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(parentContainer.Items[0]) as TreeViewItem;
                }
                else
                {
                    currentContainer = parentContainer.ItemContainerGenerator.ContainerFromIndex(1) as TreeViewItem;
                }
                if (currentContainer != null)
                {
                    currentContainer.IsSelected = true;
                    currentContainer.BringIntoView();
                    currentContainer.Focus();
                    return;
                }
                /*
                currentContainer.IsExpanded = true;
                if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    SeleccionaNodo(pathEntities, currentIndex++, currentContainer);
                }
                else
                {
                    currentContainer.ItemContainerGenerator.StatusChanged += delegate
                    {
                        SeleccionaNodo(pathEntities, currentIndex++, currentContainer);
                    };
                }
                 */
            }
            else
            {
                currentContainer.IsSelected = true;
                currentContainer.BringIntoView();
                currentContainer.Focus();
            }
        }

        private static TreeViewItem GetContainerFromPathEntity(String pathEntity, ItemsControl parentContainer)
        {
           //TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(parentContainer.Items[0]) as TreeViewItem;
            if (parentContainer.Items == null || parentContainer.Items.Count == 0) return null;
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem element = item as TreeViewItem;
                if (((String)(element.Header)).ToLower() == pathEntity.ToLower())
                {
                    TreeViewItem container = element;// parentContainer.ItemContainerGenerator.ContainerFromItem(element) as TreeViewItem;
                    return container;
                }
            }

            return null;
        }
    }
}
