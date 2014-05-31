all:
	gmcs -r:System.Windows.Forms.dll -r:System.Drawing.dll simple.cs -out:simple.exe
	./simple.exe

.PHONY: clean

clean:
	rm -rf *.exe
