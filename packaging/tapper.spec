Name:           Tapper
Version:        0.0.1
Release:        1%{?dist}
Summary:        A simple app to play sound every time you press a key on your keyboard

License:        GPLv3+
URL:            https://example.com/Tapper
Source0:        Tapper-0.0.1.tar.gz

BuildArch:      noarch

%description
This app detects the keyboard being used and every time a key is pressed,
it plays a sound defined in the config file.

%prep
%autosetup

%build

%install
mkdir -p %{buildroot}/opt/Tapper
cp -a * %{buildroot}/opt/Tapper

# Systemd unit file
mkdir -p %{buildroot}/usr/lib/systemd/system
cat > %{buildroot}/usr/lib/systemd/system/tapper.service << EOF
[Unit]
Description=Tapper Keyboard Sound Player Service
After=network.target

[Service]
Type=simple
WorkingDirectory=/opt/Tapper
ExecStart=/opt/Tapper/Tapper
Restart=always
User=nobody

[Install]
WantedBy=multi-user.target
EOF

%post
%systemd_post tapper.service

%preun
%systemd_preun tapper.service

%postun
%systemd_postun_with_restart tapper.service

%check
# Add any runtime tests here if needed

%files
%license LICENSE
%doc README.md
/opt/Tapper
/usr/lib/systemd/system/tapper.service

%changelog
* Mon Aug 04 2025 Your Name <you@example.com> - 0.0.1-1
- Initial RPM release of Tapper
