﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Parquet._3rdparty {
    internal class MeteredWriteStream : Stream {
        private readonly Stream _baseStream;

        private long _written;

        public MeteredWriteStream(Stream inner) {
            _baseStream = inner;
        }

        public long TotalBytesWritten => _written;

        public override void Flush() => _baseStream.Flush();

        public override Task FlushAsync(CancellationToken cancellationToken) => _baseStream.FlushAsync(cancellationToken);

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) => throw new NotImplementedException();

        public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        public override int Read(System.Span<byte> buffer) => throw new NotImplementedException();

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public override ValueTask<int> ReadAsync(System.Memory<byte> buffer, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public override int ReadByte() => throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) {
            _baseStream.Write(buffer, offset, count);
            _written += count;
        }

        public override void Write(System.ReadOnlySpan<byte> buffer) {
            _baseStream.Write(buffer);
            _written += buffer.Length;
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {
            await _baseStream.WriteAsync(buffer, offset, count, cancellationToken);
            _written += count;
        }

        public override async ValueTask WriteAsync(System.ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) {
            await _baseStream.WriteAsync(buffer, cancellationToken);
            _written += buffer.Length;
        }

        public override void WriteByte(byte value) {
            ++_written;
            _baseStream.WriteByte(value);
        }

        public override void CopyTo(Stream destination, int bufferSize) => throw new NotImplementedException();

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public override void Close() => _baseStream.Close();

        public override bool CanRead => _baseStream.CanRead;

        public override bool CanWrite => _baseStream.CanWrite;

        public override bool CanSeek => _baseStream.CanSeek;

        public override long Length => _baseStream.Length;

        public override bool CanTimeout => _baseStream.CanTimeout;

        public override int ReadTimeout {
            get => _baseStream.ReadTimeout;
            set => _baseStream.ReadTimeout = value;
        }

        public override int WriteTimeout {
            get => _baseStream.WriteTimeout;
            set => _baseStream.WriteTimeout = value;
        }

        public override long Position {
            get => _written;
            set => _baseStream.Position = value;
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if(disposing) {
                _baseStream.Dispose();
            }
        }

        public override async ValueTask DisposeAsync() {
            await base.DisposeAsync();
            await _baseStream.DisposeAsync();
        }
    }
}
