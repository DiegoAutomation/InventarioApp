import sounddevice as sd
import numpy as np
from collections import deque
import threading

# Configuración
SAMPLERATE = 48000  # 48kHz para baja latencia
BLOCKSIZE = 64      # 64 muestras = ~1.33ms de latencia teórica
BUFFER_SIZE = 2

# Buffer thread-safe
audio_buffer = deque(maxlen=BUFFER_SIZE * BLOCKSIZE)
buffer_lock = threading.Lock()

input_device = None
output_device = None
is_active = False

def list_devices():
    """Lista dispositivos de audio disponibles"""
    devices = sd.query_devices()
    for i, device in enumerate(devices):
        print(f"{i}: {device['name']} | IN: {device['max_input_channels']} | OUT: {device['max_output_channels']}")

def audio_callback(indata, outdata, frames, time_info, status):
    """Callback de audio con passthrough en tiempo real"""
    if status:
        print(f"⚠ Status: {status}")
    
    with buffer_lock:
        # Entrada → buffer
        audio_buffer.extend(indata[:, 0].copy())
        
        # Buffer → salida
        if len(audio_buffer) >= frames:
            out_data = np.array([audio_buffer.popleft() for _ in range(frames)])
            outdata[:, 0] = out_data
        else:
            outdata.fill(0)

def start_passthrough():
    """Inicia el passthrough de audio"""
    global is_active, input_device, output_device
    
    if is_active:
        print("❌ Ya está activo")
        return
    
    try:
        stream = sd.Stream(
            device=(input_device, output_device),
            samplerate=SAMPLERATE,
            blocksize=BLOCKSIZE,
            channels=1,
            callback=audio_callback,
            latency='low'
        )
        stream.start()
        is_active = True
        print(f"✅ Passthrough activo")
        print(f"   Input: {sd.query_devices(input_device)['name']}")
        print(f"   Output: {sd.query_devices(output_device)['name']}")
        print(f"   Latencia teórica: {(BLOCKSIZE / SAMPLERATE * 1000):.2f}ms")
        
        # Mantener el stream abierto
        input("   Presiona Enter para detener...\n")
        stream.stop()
        stream.close()
        is_active = False
        print("❌ Passthrough detenido")
        
    except Exception as e:
        print(f"Error: {e}")
        is_active = False

def select_devices():
    """Interfaz de selección de dispositivos"""
    global input_device, output_device
    
    print("\n" + "="*50)
    list_devices()
    print("="*50)
    
    try:
        input_device = int(input("Selecciona dispositivo de ENTRADA: "))
        output_device = int(input("Selecciona dispositivo de SALIDA: "))
        
        # Validar
        info_in = sd.query_devices(input_device)
        info_out = sd.query_devices(output_device)
        
        if info_in['max_input_channels'] < 1:
            print("❌ Dispositivo de entrada inválido")
            return False
        if info_out['max_output_channels'] < 1:
            print("❌ Dispositivo de salida inválido")
            return False
        
        print(f"\n✅ Configurado:")
        print(f"   Entrada: {info_in['name']}")
        print(f"   Salida: {info_out['name']}\n")
        return True
        
    except Exception as e:
        print(f"❌ Error: {e}")
        return False

if __name__ == "__main__":
    print("\n🎸 GUITAR PASSTHROUGH - Validación de Latencia\n")
    
    if select_devices():
        start_passthrough()