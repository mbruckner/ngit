/*
This code is derived from jgit (http://eclipse.org/jgit).
Copyright owners are documented in jgit's IP log.

This program and the accompanying materials are made available
under the terms of the Eclipse Distribution License v1.0 which
accompanies this distribution, is reproduced below, and is
available at http://www.eclipse.org/org/documents/edl-v10.php

All rights reserved.

Redistribution and use in source and binary forms, with or
without modification, are permitted provided that the following
conditions are met:

- Redistributions of source code must retain the above copyright
  notice, this list of conditions and the following disclaimer.

- Redistributions in binary form must reproduce the above
  copyright notice, this list of conditions and the following
  disclaimer in the documentation and/or other materials provided
  with the distribution.

- Neither the name of the Eclipse Foundation, Inc. nor the
  names of its contributors may be used to endorse or promote
  products derived from this software without specific prior
  written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections.Generic;
using System.IO;
using NGit;
using NGit.Api;
using NGit.Api.Errors;
using NGit.Util;
using Sharpen;

namespace NGit.Api
{
	/// <summary>Remove untracked files from the working tree</summary>
	/// <seealso><a
	/// *      href="http://www.kernel.org/pub/software/scm/git/docs/git-clean.html"
	/// *      >Git documentation about Clean</a></seealso>
	public class CleanCommand : GitCommand<ICollection<string>>
	{
		private ICollection<string> paths = Sharpen.Collections.EmptySet<string>();

		private bool dryRun;

		/// <param name="repo"></param>
		protected internal CleanCommand(Repository repo) : base(repo)
		{
		}

		/// <summary>
		/// Executes the
		/// <code>clean</code>
		/// command with all the options and parameters
		/// collected by the setter methods of this class. Each instance of this
		/// class should only be used for one invocation of the command (means: one
		/// call to
		/// <see cref="Call()">Call()</see>
		/// )
		/// </summary>
		/// <returns>a set of strings representing each file cleaned.</returns>
		/// <exception cref="NGit.Api.Errors.GitAPIException">NGit.Api.Errors.GitAPIException
		/// 	</exception>
		/// <exception cref="NGit.Errors.NoWorkTreeException">NGit.Errors.NoWorkTreeException
		/// 	</exception>
		public override ICollection<string> Call()
		{
			ICollection<string> files = new TreeSet<string>();
			try
			{
				StatusCommand command = new StatusCommand(repo);
				Status status = command.Call();
				foreach (string file in status.GetUntracked())
				{
					if (paths.IsEmpty() || paths.Contains(file))
					{
						if (!dryRun)
						{
							FileUtils.Delete(new FilePath(repo.WorkTree, file));
						}
						files.AddItem(file);
					}
				}
			}
			catch (IOException e)
			{
				throw new JGitInternalException(e.Message, e);
			}
			return files;
		}

		/// <summary>If paths are set, only these paths are affected by the cleaning.</summary>
		/// <remarks>If paths are set, only these paths are affected by the cleaning.</remarks>
		/// <param name="paths">the paths to set</param>
		/// <returns>
		/// 
		/// <code>this</code>
		/// </returns>
		public virtual NGit.Api.CleanCommand SetPaths(ICollection<string> paths)
		{
			this.paths = paths;
			return this;
		}

		/// <summary>If dryRun is set, the paths in question will not actually be deleted.</summary>
		/// <remarks>If dryRun is set, the paths in question will not actually be deleted.</remarks>
		/// <param name="dryRun">whether to do a dry run or not</param>
		/// <returns>
		/// 
		/// <code>this</code>
		/// </returns>
		public virtual NGit.Api.CleanCommand SetDryRun(bool dryRun)
		{
			this.dryRun = dryRun;
			return this;
		}
	}
}
