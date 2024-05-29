----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 12:42:04 PM
-- Design Name: 
-- Module Name: REGn - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity REGn is
    Generic (n : integer := 4);
	Port (
	    EN : in std_logic;
		Din : in std_logic_vector(n-1 downto 0);
		Dout : out std_logic_vector(n-1 downto 0)
	);
end REGn;

architecture Behavioral of REGn is
    signal reg : std_logic_vector(n-1 downto 0);
Begin
	Main : process (Din, EN)
		begin
			if EN = '1' then
				reg <= Din;
			end if;
		end process;
	Dout <= reg;
end Behavioral;
