# pip install Pillow

import os
from PIL import Image

folder_path = "./in"         
output_name = "atlas.png"  
cols = 4

def pack_atlas():
    files = sorted([f for f in os.listdir(folder_path) 
                    if f.endswith(('.png', '.jpg', '.jpeg')) and f != output_name])

    if not files:
        print("Frames not found!")
        return

    with Image.open(os.path.join(folder_path, files[0])) as img:
        w, h = img.size

    count = len(files)
    rows = (count + cols - 1) // cols
    atlas = Image.new("RGBA", (cols * w, rows * h), (0, 0, 0, 0))

    for i, filename in enumerate(files):
        with Image.open(os.path.join(folder_path, filename)) as img:
            x = (i % cols) * w
            y = (i // cols) * h
            atlas.paste(img, (x, y))
            print(f"Frame added: {filename} to {x}, {y}")

    atlas.save(output_name)
    print(f"\nDone! Atlas saved as {output_name}")
    print(f"Shader configuration: Columns = {cols}, Rows = {rows}")

if __name__ == "__main__":
    pack_atlas()