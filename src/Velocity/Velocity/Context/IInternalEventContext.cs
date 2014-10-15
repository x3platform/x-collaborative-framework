// Copyright 2004-2009 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace X3Platform.Velocity.Context
{
    using X3Platform.Velocity.App.Events;

    /// <summary>
    /// Interface for event support.  Note that this is a public internal
    /// interface, as it is something that will be accessed from outside
    /// of the .context package.
    /// </summary>
    public interface IInternalEventContext
    {
        EventCartridge EventCartridge { get; }

        EventCartridge AttachEventCartridge(EventCartridge eventCartridge);
    }
}