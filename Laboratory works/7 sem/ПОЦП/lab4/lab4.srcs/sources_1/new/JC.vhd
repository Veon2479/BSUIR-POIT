----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 03:28:29 PM
-- Design Name: 
-- Module Name: JC - Behavioral
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

entity JC is
    Generic (i : integer := 2);
    Port (
        CLK: in std_logic;
        RST: in std_logic;
        LS: in std_logic; --at '0' - setting init state 
        Pin: in std_logic_vector(0 to 2**i-1);
        Pout: out std_logic_vector(0 to 2**i-1)
    );
end JC;

architecture Behavioral of JC is
    signal sreg, sdat: std_logic_vector(0 to 2**i-1);
begin
    
    main: process(CLK, RST, sdat)
    begin
        if RST = '1' then
            sreg <= (others => '0');
        elsif rising_edge(CLK) then 
            sreg <= sdat;        
        end if;
    end process;
    
    data: process(LS, Pin, sreg)
    begin 
        if LS = '0' then 
            sdat <= Pin;
        else
            sdat <= not (sreg(2**i-1)) & sreg (0 to 2**i-2);
        end if;
    end process;

    Pout <= sdat;

end Behavioral;
