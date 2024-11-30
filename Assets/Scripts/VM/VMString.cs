/*
 * (C) 2023 Radrat Softworks
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Nofun.VM
{
    public struct VMString
    {
        private UInt32 address;

        public VMString(uint address)
        {
            this.address = address;
        }

        public uint Address => address;

        public string Get(VMMemory memory, bool isUtf16 = false)
        {
            string value = "";
            uint curAddr = address;

            do
            {
                char val = '\0';
                if (isUtf16)
                {
                    val = (char)memory.ReadMemory16(curAddr);
                    curAddr += 2;
                } else
                {
                    val = (char)memory.ReadMemory8(curAddr++);
                }
                if (val == '\0')
                {
                    break;
                }
                value += val;
            } while (true);

            return value;
        }

        public void Set(VMMemory memory, string value, bool isUtf16 = false)
        {
            byte[] bytes;
            if (isUtf16)
            {
                bytes = Encoding.Unicode.GetBytes(value);
            } else
            {
                bytes = Encoding.ASCII.GetBytes(value);
            }
                
            var destSpan = memory.GetMemorySpan((int)address, bytes.Length + 1);
            bytes.AsSpan().CopyTo(destSpan);

            destSpan[bytes.Length] = 0;
        }
    }
}