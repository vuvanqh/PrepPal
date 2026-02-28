using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.ServiceContracts;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}