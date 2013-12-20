﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using NuGet.Services.Jobs.Monitoring;

namespace NuGet.Services.Jobs
{
    public class InvocationContext
    {
        private const string InvocationIdDataName = "_NuGet_Services_Jobs_Invocation_Id";

        private InvocationLogCapture _capture;

        public Invocation Invocation { get; private set; }
        public InvocationQueue Queue { get; private set; }
        public CancellationToken CancelToken { get; private set; }

        public InvocationContext(Invocation invocation, InvocationQueue queue)
            : this(invocation, queue, CancellationToken.None)
        {
        }

        public InvocationContext(Invocation invocation, InvocationQueue queue, CancellationToken cancelToken)
        {
            Invocation = invocation;
            Queue = queue;
            CancelToken = cancelToken;
        }

        public InvocationContext(Invocation invocation, InvocationQueue queue, CancellationToken cancelToken, InvocationLogCapture capture)
            : this(invocation, queue, cancelToken)
        {
            _capture = capture;
        }

        public static Guid GetCurrentInvocationId()
        {
            var obj = CallContext.LogicalGetData(InvocationIdDataName);
            return obj == null ? Guid.Empty : (Guid)obj;
        }

        public static void SetCurrentInvocationId(Guid id)
        {
            CallContext.LogicalSetData(InvocationIdDataName, id);
        }

        public void SetJob(JobDescription jobdef, JobBase job)
        {
            if (_capture != null)
            {
                _capture.SetJob(jobdef, job);   
            }
        }
    }
}
