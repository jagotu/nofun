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

using Nofun.Driver.Unity.Graphics;
using Nofun.Util;
using Nofun.VM;
using System;

using UnityEngine;
using UnityEngine.UIElements;

namespace Nofun.Module.VMGP3D
{
    [Module]
    public partial class VMGP3D
    {
        [ModuleCall]
        private void vVectorTransformV3(VMPtr<NativeVector3D> source, VMPtr<NativeVector3D> destination, int count)
        {
            Span<NativeVector3D> sourceSpan = source.AsSpan(system.Memory, count);
            Span<NativeVector3D> destSpan = source.AsSpan(system.Memory, count);
        
            for (int i = 0; i < count; i++)
            {
                destSpan[i] = currentMatrix.MultiplyPoint3x4(sourceSpan[i].ToUnity()).ToMophun();
            }
        }

        [ModuleCall]
        private void vVectorAdd(VMPtr<NativeVector3D> dest, VMPtr<NativeVector3D> lhs, VMPtr<NativeVector3D> rhs)
        {
            NativeVector3D lhsValue = lhs.Read(system.Memory);
            NativeVector3D rhsValue = rhs.Read(system.Memory);

            dest.Write(system.Memory, new NativeVector3D()
            {
                fixedX = lhsValue.fixedX + rhsValue.fixedX,
                fixedY = lhsValue.fixedY + rhsValue.fixedY,
                fixedZ = lhsValue.fixedZ + rhsValue.fixedZ,
            });
        }

        [ModuleCall]
        private void vVectorSub(VMPtr<NativeVector3D> dest, VMPtr<NativeVector3D> lhs, VMPtr<NativeVector3D> rhs)
        {
            NativeVector3D lhsValue = lhs.Read(system.Memory);
            NativeVector3D rhsValue = rhs.Read(system.Memory);

            dest.Write(system.Memory, new NativeVector3D()
            {
                fixedX = lhsValue.fixedX - rhsValue.fixedX,
                fixedY = lhsValue.fixedY - rhsValue.fixedY,
                fixedZ = lhsValue.fixedZ - rhsValue.fixedZ,
            });
        }

        // It just multiplies. Dot and product is separate function
        [ModuleCall]
        private void vVectorMul(VMPtr<NativeVector3D> dest, VMPtr<NativeVector3D> lhs, VMPtr<NativeVector3D> rhs)
        {
            NativeVector3D lhsValue = lhs.Read(system.Memory);
            NativeVector3D rhsValue = rhs.Read(system.Memory);

            dest.Write(system.Memory, new NativeVector3D()
            {
                fixedX = FixedUtil.FloatToFixed(FixedUtil.FixedToFloat(lhsValue.fixedX) * FixedUtil.FixedToFloat(rhsValue.fixedX)),
                fixedY = FixedUtil.FloatToFixed(FixedUtil.FixedToFloat(lhsValue.fixedY) * FixedUtil.FixedToFloat(rhsValue.fixedY)),
                fixedZ = FixedUtil.FloatToFixed(FixedUtil.FixedToFloat(lhsValue.fixedZ) * FixedUtil.FixedToFloat(rhsValue.fixedZ)),
            });
        }

        [ModuleCall]
        private void vVectorNormalize(VMPtr<NativeVector3D> dest)
        {
            NativeVector3D destValue = dest.Read(system.Memory);
            float x = FixedUtil.FixedToFloat(destValue.fixedX);
            float y = FixedUtil.FixedToFloat(destValue.fixedY);
            float z = FixedUtil.FixedToFloat(destValue.fixedZ);

            double length = Math.Sqrt(z * z + x * x + y * y);
            dest.Write(system.Memory, new NativeVector3D()
            {
                fixedX = FixedUtil.FloatToFixed((float)(x / length)),
                fixedY = FixedUtil.FloatToFixed((float)(y / length)),
                fixedZ = FixedUtil.FloatToFixed((float)(z / length)),
            });
        }

        [ModuleCall]
        private void vCrossProduct(VMPtr<NativeVector3D> dest, VMPtr<NativeVector3D> lhs, VMPtr<NativeVector3D> rhs)
        {
            NativeVector3D lhsValue = lhs.Read(system.Memory);
            NativeVector3D rhsValue = rhs.Read(system.Memory);

            float lhsX = FixedUtil.FixedToFloat(lhsValue.fixedX);
            float lhsY = FixedUtil.FixedToFloat(lhsValue.fixedY);
            float lhsZ = FixedUtil.FixedToFloat(lhsValue.fixedZ);

            float rhsX = FixedUtil.FixedToFloat(rhsValue.fixedX);
            float rhsY = FixedUtil.FixedToFloat(rhsValue.fixedY);
            float rhsZ = FixedUtil.FixedToFloat(rhsValue.fixedZ);

            dest.Write(system.Memory, new NativeVector3D()
            {
                fixedX = FixedUtil.FloatToFixed((float)(lhsY * rhsZ - lhsZ * rhsY)),
                fixedY = FixedUtil.FloatToFixed((float)(lhsZ * rhsX - lhsX * rhsZ)),
                fixedZ = FixedUtil.FloatToFixed((float)(lhsX * rhsY - lhsY * rhsX)),
            });
        }

        [ModuleCall]
        private int vDotProduct(VMPtr<NativeVector3D> lhs, VMPtr<NativeVector3D> rhs)
        {
            NativeVector3D lhsValue = lhs.Read(system.Memory);
            NativeVector3D rhsValue = rhs.Read(system.Memory);

            float lhsX = FixedUtil.FixedToFloat(lhsValue.fixedX);
            float lhsY = FixedUtil.FixedToFloat(lhsValue.fixedY);
            float lhsZ = FixedUtil.FixedToFloat(lhsValue.fixedZ);

            float rhsX = FixedUtil.FixedToFloat(rhsValue.fixedX);
            float rhsY = FixedUtil.FixedToFloat(rhsValue.fixedY);
            float rhsZ = FixedUtil.FixedToFloat(rhsValue.fixedZ);

            return FixedUtil.FloatToFixed(lhsX * rhsX + lhsY * rhsY + lhsZ * rhsZ);
        }

        [ModuleCall]
        [UncertainImplementation("It outputs a Vector4, while viewport scale only affect x and y. Need to verify with original implementation.")]
        private void vVectorProjectV3(VMPtr<NativeVector4D> destPtr, VMPtr<NativeVector3D> sourcePtr, int count)
        {
            Span<NativeVector3D> source = sourcePtr.AsSpan(system.Memory, count);
            Span<NativeVector4D> dest = destPtr.AsSpan(system.Memory, count);

            NRectangle viewportRect = system.GraphicDriver.Viewport;

            for (int i = 0; i < count; i++)
            {
                Vector3 sourceV3 = source[i].ToUnity();
                Vector4 projected = projectionMatrix * new Vector4(sourceV3.x, sourceV3.y, sourceV3.z, 1.0f);

                // Map to 0..1
                projected /= projected.w;
                projected += Vector4.one;
                projected.Scale(new Vector4(viewportRect.width * 0.5f, viewportRect.height * 0.5f, 1.0f, 1.0f));

                dest[i] = (projected + new Vector4(viewportRect.x, viewportRect.y, 0.0f, 0.0f)).ToMophun();
            }
        }
    }
}