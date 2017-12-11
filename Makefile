.PHONY: all

OS := $(shell uname)
MONO_PATH := $(shell which mono 2>/dev/null)
ifeq (,$(MONO_PATH))
$(error mono is required for building net45 applications, but is not installed)
endif
ifeq ($(OS),Darwin)
	MONO_LIB := $(MONO_PATH)/../../lib/mono/4.5/
else ifeq ($(OS),Linux)
	MONO_LIB := $(MONO_PATH)/../../lib/mono/4.5/
endif

all: clean build

build:
	FrameworkPathOverride=$(MONO_LIB) dotnet build

clean:
	rm -rf bin/ obj/