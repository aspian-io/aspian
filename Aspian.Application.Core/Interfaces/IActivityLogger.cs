using System;
using System.Threading.Tasks;
using Aspian.Domain.ActivityModel;

namespace Aspian.Application.Core.Interfaces
{
    public interface IActivityLogger
    {
        /// <summary>
        /// Add an activity in database by getting the <paramref name="siteId"/> and its <paramref name="code"/>, <paramref name="severity"/>, <paramref name="objectName"/> and <paramref name="message"/>.
        /// </summary>
        /// <param name="siteId" >The guid of current site.</param>
        /// <param name="code" >The code of an activity.</param>
        /// <param name="severity" >The severity of an activity.</param>
        /// <param name="objectName" >The object name of an activity.</param>
        /// <param name="message" >The message or description of an activity.</param>
         Task LogActivity(Guid siteId, ActivityCodeEnum code, ActivitySeverityEnum severity, ActivityObjectEnum objectName, string message);
    }
}