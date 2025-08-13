# Git Workflow Overview

Dokumen ini ngejelasin cara tim dev game ini pakai Git — dari struktur branch, gaya commit, sampai pull request. Tujuannya biar kerja tim jadi lancar, rapi, dan progress develop game-nya bisa diawasi bareng-bareng.

<br>

## Git Branching Strategy

Strategi branching ini dibuat biar develop fitur, debug, dan eksperimen game tetap terstruktur tapi fleksibel.

### Main Branches

#### `main`

* Ini branch buat **build paling stabil** yang siap di-publish (misal buat demo atau rilis Steam).
* Kode di sini udah lewat testing dan review.
* Cuma merge dari `develop` yang udah fix dan stabil.

#### `develop`

* Tempat semua **fitur baru dan bug fix** digabung dulu sebelum ke `main`.
* Branch ini aktif dipakai selama produksi game berlangsung.
* Jadi basis buat milestone atau playtest build.

#### `playground`

* Buat **eksperimen bebas**, nyoba fitur gila, atau debug aneh.
* Boleh bikin sistem baru, ngetes mechanic, atau bikin shader random di sini.
* **Catatan**: Jangan langsung merge ke `develop`. Harus di-review dan dites dulu.

<br>

## Naming Convention

Biar semua branch dan commit gampang dimengerti sama semua dev (coder, artist, audio, dsb), kita pakai format yang konsisten.

### Branch Names

`<nama>/<tipe>/<detail>`

* **Format**:

  * `nama`: Nama dev (contoh: `vinchen`, `arya`)
  * `tipe`: Jenis kerjaan (`feat`, `fix`, `style`, dst)
  * `detail`: Ringkasan tugas (contoh: `enemy-ai`, `pause-menu`)

* **Tipe umum**:

  * `feat` — fitur baru (ex: sistem combat, cutscene)
  * `fix` — perbaikan bug (ex: karakter nyangkut, fps drop)
  * `docs` — dokumentasi (ex: setup project, input mapping)
  * `style` — perapihan kode (formatting, naming)
  * `refactor` — perombakan logika (tanpa ubah behavior)
  * `test` — nambah atau edit testing (kalau ada automated test)

* **Contoh**:

  * `vinchen/feat/boss-attack-pattern`
  * `arya/fix/player-jump-bug`

### Commit Messages

`<tipe>(opsional-cakupan): deskripsi`

* **Format**:

  * `tipe`: Jenis commit (lihat daftar di atas)
  * `cakupan`: (opsional) bagian dari game/codebase yang diubah
  * `deskripsi`: singkat & padat, jangan drama

* **Contoh**:

  * `feat: tambahin UI health bar`
  * `fix(audio): looping BGM gak berhenti`
  * `refactor(movement): bersihin logika gerak player`

### Pull Request (Title)

`<tipe>(opsional-cakupan): deskripsi singkat`

* Sama aja kayak format commit, tapi buat judul PR.

* **Contoh**:

  * `feat: sistem damage musuh udah jadi`
  * `fix(ai): enemy stuck di dinding`
  * `docs: nambah cara export ke Android`

### Pull Request (Description)

```
## Description
Jelaskan fitur atau bug yang ditangani.

## Changes Made
* Poin-poin apa aja yang diubah/dibuat

## Screenshots (kalau ada)
Upload screenshot / gif / screen recording hasil perubahan
```

* **Contoh**:

```
## Description
Tambah sistem serangan untuk boss terakhir, Rudy the Reindeer.

## Changes Made
* Animasi dan state transition attack
* Logic dash dan area slam
* Tambah efek suara dan partikel salju
* Update UI ketika boss menyerang

## Screenshots
(gif animasi dash dan serangan slam)
```
