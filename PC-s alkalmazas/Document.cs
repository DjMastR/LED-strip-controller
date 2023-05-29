using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_program
{
    public class Document
    {
        List<IView> views = new List<IView>();

        public void AttachView(IView view)
        {
            views.Add(view);
            view.UpdateView();
        }

        public void DetachView(IView view)
        {
            views.Remove(view);
        }

        public bool HasAnyView()
        {
            return views.Count > 0;
        }

        protected void UpdateAllViews()
        {
            foreach (IView view in views)
            {
                view.UpdateView();
            }
        }

        public virtual void LoadDocument(string filePath) { }

        public virtual void SaveDocument(string filePath) { }
    }
}
