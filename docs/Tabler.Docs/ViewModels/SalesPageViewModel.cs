﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class SalesPageViewModel : IRefresh
    {
        public bool IsLoading { get; set; }
        public Sale Sale { get; set; }
        public List<GroupLine> Lines { get; set; }
        public GroupLine GroupLine { get; set; }
        public List<Customer> Customers { get; set; }
        public bool IsInCategory => (GroupLine?.Category?.Id ?? 0) > 0;

        public event EventHandler Refreshed;

        public int PageSize
        {
            get
            {
                int size = GroupLine.Products.Count / 4;
                if (size < 4)
                {
                    size = 4;
                }

                return size;
            }
        }

        public SalesPageViewModel()
        {
            this.GroupLine = new GroupLine(new Category());
            this.Customers = new List<Customer>();
            this.Lines = new List<GroupLine>();
            this.Sale = new Sale();
        }

        public async Task Refresh()
        {
            try
            {
                if (IsLoading)
                {
                    return;
                }
                IsLoading = true;
                Customers.Clear();
                Customers.AddRange(await Customer.GetAll());
                this.Lines.Clear();
                foreach (Category linea in await Category.GetAll())
                {
                    var gropup = new GroupLine(linea);
                    await gropup.Refresh();
                    this.Lines.Add(gropup);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsLoading = false;
                Refreshed?.Invoke(this, EventArgs.Empty);
            }

        }


        public async Task LoadLine(GroupLine line)
        {
            IsLoading = true;
            await Task.Yield();
            await line.Refresh();
            IsLoading = false;
        }
    }
}